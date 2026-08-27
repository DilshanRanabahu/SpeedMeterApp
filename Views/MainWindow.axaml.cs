using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using SpeedMeterApp.ViewModels;

namespace SpeedMeterApp.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            BeginMoveDrag(e);
    }

    private void Window_Loaded(object? sender, RoutedEventArgs e)
    {
        if (Screens.Primary != null)
        {
            var desktopWorkingArea = Screens.Primary.WorkingArea;
            this.Position = new PixelPoint(
                desktopWorkingArea.Right - (int)this.Width - 10,
                desktopWorkingArea.Bottom - (int)this.Height - 10);
        }
    }

    private void ShowGraph_Click(object? sender, RoutedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            foreach (var window in desktop.Windows)
            {
                if (window is GraphWindow)
                {
                    window.Activate();
                    return;
                }
            }
        }

        var graphWindow = new GraphWindow
        {
            DataContext = App.GraphVM
        };
        graphWindow.Show();
    }

    private void Exit_Click(object? sender, RoutedEventArgs e)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
}