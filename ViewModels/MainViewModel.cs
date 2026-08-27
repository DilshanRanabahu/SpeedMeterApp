using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpeedMeterApp.Services;

namespace SpeedMeterApp.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly NetworkDataService _dataService;

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

    public MainViewModel(NetworkDataService dataService)
    {
        _dataService = dataService;
        _dataService.TrafficUpdated += OnTrafficUpdated;
    }

    private void OnTrafficUpdated(long receivedDelta, long sentDelta)
    {
        FormatSpeed(receivedDelta, out string downSpeed, out string downUnit);
        FormatSpeed(sentDelta, out string upSpeed, out string upUnit);

        DownloadSpeed = downSpeed;
        DownloadUnit = downUnit;
        UploadSpeed = upSpeed;
        UploadUnit = upUnit;
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
