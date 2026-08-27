using System.Linq;
using System.Net.NetworkInformation;

namespace SpeedMeterApp;

public class NetworkMonitor
{
    public NetworkMonitor()
    {
    }


    public (long BytesReceived, long BytesSent) GetTotalNetworkTraffic()
    {
        long bytesReceived = 0;
        long bytesSent = 0;

        var interfaces = NetworkInterface.GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == OperationalStatus.Up && 
                          nic.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                          nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel);

        foreach (var nic in interfaces)
        {
            try
            {
                var stats = nic.GetIPStatistics();
                bytesReceived += stats.BytesReceived;
                bytesSent += stats.BytesSent;
            }
            catch
            {

            }
        }

        return (bytesReceived, bytesSent);
    }
}
