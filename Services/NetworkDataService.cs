using System;
using Avalonia.Threading;
using SpeedMeterApp.Models;

namespace SpeedMeterApp.Services;

public class NetworkDataService
{
    private readonly NetworkMonitor _networkMonitor;
    private readonly DispatcherTimer _timer;
    
    private long _lastBytesReceived = 0;
    private long _lastBytesSent = 0;

    public event Action<long, long>? TrafficUpdated;

    public NetworkDataService()
    {
        _networkMonitor = new NetworkMonitor();
        
        var initialTraffic = _networkMonitor.GetTotalNetworkTraffic();
        _lastBytesReceived = initialTraffic.BytesReceived;
        _lastBytesSent = initialTraffic.BytesSent;

        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
    }

    public void Start()
    {
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        var currentTraffic = _networkMonitor.GetTotalNetworkTraffic();

        long receivedDelta = currentTraffic.BytesReceived - _lastBytesReceived;
        long sentDelta = currentTraffic.BytesSent - _lastBytesSent;

        if (receivedDelta < 0) receivedDelta = 0;
        if (sentDelta < 0) sentDelta = 0;

        _lastBytesReceived = currentTraffic.BytesReceived;
        _lastBytesSent = currentTraffic.BytesSent;

        TrafficUpdated?.Invoke(receivedDelta, sentDelta);
    }
}
