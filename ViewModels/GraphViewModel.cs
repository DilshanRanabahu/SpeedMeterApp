using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using SpeedMeterApp.Services;

namespace SpeedMeterApp.ViewModels;

public class GraphViewModel : INotifyPropertyChanged
{
    private readonly NetworkDataService _dataService;

    public List<double> DownloadHistory { get; } = new List<double>();
    public List<double> UploadHistory { get; } = new List<double>();
    
    private double _maxSpeedValue = 1024;
    public double MaxSpeedValue 
    { 
        get => _maxSpeedValue; 
        private set { if (_maxSpeedValue != value) { _maxSpeedValue = value; OnPropertyChanged(); } } 
    }

    private string _maxSpeedText = "1 KB/s";
    public string MaxSpeedText
    {
        get => _maxSpeedText;
        private set { if (_maxSpeedText != value) { _maxSpeedText = value; OnPropertyChanged(); } }
    }

    private string _maxSpeed75Text = "0.75 KB/s";
    public string MaxSpeed75Text
    {
        get => _maxSpeed75Text;
        private set { if (_maxSpeed75Text != value) { _maxSpeed75Text = value; OnPropertyChanged(); } }
    }

    private string _maxSpeed50Text = "0.5 KB/s";
    public string MaxSpeed50Text
    {
        get => _maxSpeed50Text;
        private set { if (_maxSpeed50Text != value) { _maxSpeed50Text = value; OnPropertyChanged(); } }
    }

    private string _maxSpeed25Text = "0.25 KB/s";
    public string MaxSpeed25Text
    {
        get => _maxSpeed25Text;
        private set { if (_maxSpeed25Text != value) { _maxSpeed25Text = value; OnPropertyChanged(); } }
    }

    private string _currentDownloadText = "0 B/s";
    public string CurrentDownloadText
    {
        get => _currentDownloadText;
        private set { if (_currentDownloadText != value) { _currentDownloadText = value; OnPropertyChanged(); } }
    }

    private string _currentUploadText = "0 B/s";
    public string CurrentUploadText
    {
        get => _currentUploadText;
        private set { if (_currentUploadText != value) { _currentUploadText = value; OnPropertyChanged(); } }
    }

    public event Action? GraphUpdated;

    public GraphViewModel(NetworkDataService dataService)
    {
        _dataService = dataService;
        
        for(int i = 0; i < 60; i++)
        {
            DownloadHistory.Add(0);
            UploadHistory.Add(0);
        }

        _dataService.TrafficUpdated += OnTrafficUpdated;
    }

    private void OnTrafficUpdated(long receivedDelta, long sentDelta)
    {
        DownloadHistory.Add(receivedDelta);
        UploadHistory.Add(sentDelta);

        if (DownloadHistory.Count > 60) DownloadHistory.RemoveAt(0);
        if (UploadHistory.Count > 60) UploadHistory.RemoveAt(0);

        CurrentDownloadText = FormatSpeed(receivedDelta);
        CurrentUploadText = FormatSpeed(sentDelta);

        UpdateMaxSpeed();
        GraphUpdated?.Invoke();
    }

    private void UpdateMaxSpeed()
    {
        double maxDown = DownloadHistory.Count > 0 ? DownloadHistory.Max() : 0;
        double maxUp = UploadHistory.Count > 0 ? UploadHistory.Max() : 0;
        double currentMax = Math.Max(maxDown, maxUp);
        
        currentMax = Math.Max(currentMax * 1.2, 1024);
        MaxSpeedValue = currentMax;
        
        MaxSpeedText = FormatSpeed((long)MaxSpeedValue);
        MaxSpeed75Text = FormatSpeed((long)(MaxSpeedValue * 0.75));
        MaxSpeed50Text = FormatSpeed((long)(MaxSpeedValue * 0.50));
        MaxSpeed25Text = FormatSpeed((long)(MaxSpeedValue * 0.25));
    }

    private string FormatSpeed(long bytesPerSecond)
    {
        if (bytesPerSecond >= 1024 * 1024)
            return (bytesPerSecond / (1024.0 * 1024.0)).ToString("0.00") + " MB/s";
        if (bytesPerSecond >= 1024)
            return (bytesPerSecond / 1024.0).ToString("0") + " KB/s";
        return bytesPerSecond.ToString("0") + " B/s";
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
