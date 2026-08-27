using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SpeedMeterApp.Services;
using SpeedMeterApp.ViewModels;
using SpeedMeterApp.Views;

namespace SpeedMeterApp;

public partial class App : Application
{
    public static NetworkDataService DataService { get; private set; } = null!;
    public static MainViewModel MainVM { get; private set; } = null!;
    public static GraphViewModel GraphVM { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DataService = new NetworkDataService();
            MainVM = new MainViewModel(DataService);
            GraphVM = new GraphViewModel(DataService);

            DataService.Start();
            
            EnableWindowsStartup();

            desktop.MainWindow = new MainWindow
            {
                DataContext = MainVM
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void EnableWindowsStartup()
    {
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
        {
            try
            {
                using var runKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                if (runKey != null)
                {
                    string appName = "SpeedMeterApp";
                    string? appPath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;
                    if (!string.IsNullOrEmpty(appPath))
                    {
                        runKey.SetValue(appName, $"\"{appPath}\"");
                    }
                }
            }
            catch
            {
                // Ignore any potential registry access errors silently
            }
        }
    }
}

