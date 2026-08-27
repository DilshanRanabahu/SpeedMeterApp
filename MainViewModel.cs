using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace SpeedMeterApp;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly NetworkMonitor _networkMonitor;
    private readonly DispatcherTimer _timer;
    
    private long _lastBytesReceived = 0;
    private long _lastBytesSent = 0;

    private string _downloadSpeed = "0.00";
    private string _uploadSpeed = "0.00";
    private string _downloadUnit = "KB/s";
    private string _uploadUnit = "KB/s";

    public string DownloadSpeed
    {
        get => _downloadSpeed;
        set { if (_downloadSpeed != value) { _downloadSpeed = value; OnPropertyChanged(); } }
    }

    public string UploadSpeed
    {
        get => _uploadSpeed;
        set { if (_uploadSpeed != value) { _uploadSpeed = value; OnPropertyChanged(); } }
    }

    public string DownloadUnit
    {
        get => _downloadUnit;
        set { if (_downloadUnit != value) { _downloadUnit = value; OnPropertyChanged(); } }
    }

    public string UploadUnit
    {
        get => _uploadUnit;
        set { if (_uploadUnit != value) { _uploadUnit = value; OnPropertyChanged(); } }
    }

    public MainViewModel()
    {
        _networkMonitor = new NetworkMonitor();
        
        var initialTraffic = _networkMonitor.GetTotalNetworkTraffic();
        _lastBytesReceived = initialTraffic.BytesReceived;
        _lastBytesSent = initialTraffic.BytesSent;

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        var currentTraffic = _networkMonitor.GetTotalNetworkTraffic();

        long receivedDelta = currentTraffic.BytesReceived - _lastBytesReceived;
        long sentDelta = currentTraffic.BytesSent - _lastBytesSent;

        if (receivedDelta < 0) receivedDelta = 0;
        if (sentDelta < 0) sentDelta = 0;

        FormatSpeed(receivedDelta, out string downSpeed, out string downUnit);
        FormatSpeed(sentDelta, out string upSpeed, out string upUnit);

        DownloadSpeed = downSpeed;
        DownloadUnit = downUnit;
        UploadSpeed = upSpeed;
        UploadUnit = upUnit;

        _lastBytesReceived = currentTraffic.BytesReceived;
        _lastBytesSent = currentTraffic.BytesSent;
    }

    private void FormatSpeed(long bytesPerSecond, out string speed, out string unit)
    {
        if (bytesPerSecond >= 1024 * 1024)
        {
            speed = (bytesPerSecond / (1024.0 * 1024.0)).ToString("0.00");
            unit = "MB/s";
        }
        else if (bytesPerSecond >= 1024)
        {
            speed = (bytesPerSecond / 1024.0).ToString("0");
            unit = "KB/s";
        }
        else
        {
            speed = bytesPerSecond.ToString("0");
            unit = " B/s";
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
