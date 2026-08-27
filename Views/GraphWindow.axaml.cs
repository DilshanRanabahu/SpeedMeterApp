using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using SpeedMeterApp.ViewModels;

namespace SpeedMeterApp.Views;

public partial class GraphWindow : Window
{
    private GraphViewModel? _viewModel;

    public GraphWindow()
    {
        InitializeComponent();
        this.Closed += GraphWindow_Closed;
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

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

    private void GraphWindow_Closed(object? sender, EventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.GraphUpdated -= DrawGraph;
        }
    }

    private void GraphCanvas_SizeChanged(object? sender, SizeChangedEventArgs e)
    {
        double h = e.NewSize.Height;
        double w = e.NewSize.Width;
        
        GridLine75.StartPoint = new Point(0, h * 0.25);
        GridLine75.EndPoint = new Point(w, h * 0.25);

        GridLine50.StartPoint = new Point(0, h * 0.50);
        GridLine50.EndPoint = new Point(w, h * 0.50);

        GridLine25.StartPoint = new Point(0, h * 0.75);
        GridLine25.EndPoint = new Point(w, h * 0.75);
        
        DrawGraph();
    }

    private void DrawGraph()
    {
        if (_viewModel == null || GraphCanvas.Bounds.Width == 0 || GraphCanvas.Bounds.Height == 0)
            return;

        double width = GraphCanvas.Bounds.Width;
        double height = GraphCanvas.Bounds.Height;
        double maxSpeed = _viewModel.MaxSpeedValue;

        if (maxSpeed <= 0) maxSpeed = 1;

        // X-axis step (60 seconds history)
        double xStep = width / 59.0;

        var downloadPoints = new List<Point>();
        var uploadPoints = new List<Point>();

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

        CurrentDownloadLine.StartPoint = new Point(0, lastDownY);
        CurrentDownloadLine.EndPoint = new Point(width, lastDownY);

        CurrentDownloadText.Text = _viewModel.CurrentDownloadText;
        Canvas.SetTop(CurrentDownloadText, lastDownY - 16);
        Canvas.SetLeft(CurrentDownloadText, (width / 2) - 25);

        CurrentUploadLine.StartPoint = new Point(0, lastUpY);
        CurrentUploadLine.EndPoint = new Point(width, lastUpY);

        CurrentUploadText.Text = _viewModel.CurrentUploadText;
        Canvas.SetTop(CurrentUploadText, lastUpY - 16);
        Canvas.SetLeft(CurrentUploadText, (width / 2) - 25);
    }

    private void CloseButton_Click(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void Window_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            BeginMoveDrag(e);
    }
}
