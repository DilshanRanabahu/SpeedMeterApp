using System.Configuration;
using System.Data;
using System.Windows;
using SpeedMeterApp.Services;
using SpeedMeterApp.ViewModels;
using SpeedMeterApp.Views;

namespace SpeedMeterApp;


public partial class App : Application
{
    public static NetworkDataService DataService { get; private set; } = null!;
    public static MainViewModel MainVM { get; private set; } = null!;
    public static GraphViewModel GraphVM { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        DataService = new NetworkDataService();
        MainVM = new MainViewModel(DataService);
        GraphVM = new GraphViewModel(DataService);

        DataService.Start();

        var mainWindow = new MainWindow
        {
            DataContext = MainVM
        };
        mainWindow.Show();
    }
}

