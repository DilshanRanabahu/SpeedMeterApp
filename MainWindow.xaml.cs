using System.Windows;

namespace SpeedMeterApp;

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
}