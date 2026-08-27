using System.Windows;
using SpeedMeterApp.ViewModels;

namespace SpeedMeterApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        this.MouseLeftButtonDown += (s, e) => this.DragMove();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var desktopWorkingArea = SystemParameters.WorkArea;
        this.Left = desktopWorkingArea.Right - this.Width - 10;
        this.Top = desktopWorkingArea.Bottom - this.Height - 10;
    }

    private void ShowGraph_Click(object sender, RoutedEventArgs e)
    {
        // Only open one graph window
        foreach (Window window in Application.Current.Windows)
        {
            if (window is GraphWindow)
            {
                window.Activate();
                return;
            }
        }

        var graphWindow = new GraphWindow
        {
            DataContext = App.GraphVM
        };
        graphWindow.Show();
    }
}