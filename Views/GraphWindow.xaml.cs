using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SpeedMeterApp.ViewModels;

namespace SpeedMeterApp.Views;

public partial class GraphWindow : Window
{
    private GraphViewModel? _viewModel;

    public GraphWindow()
    {
        InitializeComponent();
        this.DataContextChanged += GraphWindow_DataContextChanged;
        this.Closed += GraphWindow_Closed;
    }

    private void GraphWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.GraphUpdated -= DrawGraph;
        }

        if (this.DataContext is GraphViewModel vm)
        {
            _viewModel = vm;
            _viewModel.GraphUpdated += DrawGraph;
            DrawGraph();
        }
    }

    private void GraphWindow_Closed(object? sender, System.EventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.GraphUpdated -= DrawGraph;
        }
    }

    private void GraphCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        double h = e.NewSize.Height;
        double w = e.NewSize.Width;
        
        GridLine75.Y1 = h * 0.25;
        GridLine75.Y2 = h * 0.25;
        GridLine75.X2 = w;

        GridLine50.Y1 = h * 0.50;
        GridLine50.Y2 = h * 0.50;
        GridLine50.X2 = w;

        GridLine25.Y1 = h * 0.75;
        GridLine25.Y2 = h * 0.75;
        GridLine25.X2 = w;
        
        DrawGraph();
    }

    private void DrawGraph()
    {
        if (_viewModel == null || GraphCanvas.ActualWidth == 0 || GraphCanvas.ActualHeight == 0)
            return;

        double width = GraphCanvas.ActualWidth;
        double height = GraphCanvas.ActualHeight;
        double maxSpeed = _viewModel.MaxSpeedValue;

        if (maxSpeed <= 0) maxSpeed = 1;

        // X-axis step (60 seconds history)
        double xStep = width / 59.0;

        var downloadPoints = new PointCollection();
        var uploadPoints = new PointCollection();

        var downHistory = _viewModel.DownloadHistory;
        var upHistory = _viewModel.UploadHistory;

        double lastDownY = 0;
        double lastUpY = 0;

        for (int i = 0; i < downHistory.Count; i++)
        {
            double x = i * xStep;
            
            // Invert Y axis because Canvas (0,0) is top-left
            double downY = height - ((downHistory[i] / maxSpeed) * height);
            double upY = height - ((upHistory[i] / maxSpeed) * height);

            downloadPoints.Add(new Point(x, downY));
            uploadPoints.Add(new Point(x, upY));

            if (i == downHistory.Count - 1)
            {
                lastDownY = downY;
                lastUpY = upY;
            }
        }

        DownloadLine.Points = downloadPoints;
        UploadLine.Points = uploadPoints;

        CurrentDownloadLine.X1 = 0;
        CurrentDownloadLine.Y1 = lastDownY;
        CurrentDownloadLine.X2 = width;
        CurrentDownloadLine.Y2 = lastDownY;

        CurrentDownloadText.Text = _viewModel.CurrentDownloadText;
        Canvas.SetTop(CurrentDownloadText, lastDownY - 16);
        Canvas.SetLeft(CurrentDownloadText, (width / 2) - 25);

        CurrentUploadLine.X1 = 0;
        CurrentUploadLine.Y1 = lastUpY;
        CurrentUploadLine.X2 = width;
        CurrentUploadLine.Y2 = lastUpY;

        CurrentUploadText.Text = _viewModel.CurrentUploadText;
        Canvas.SetTop(CurrentUploadText, lastUpY - 16);
        Canvas.SetLeft(CurrentUploadText, (width / 2) - 25);
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        this.DragMove();
    }
}
