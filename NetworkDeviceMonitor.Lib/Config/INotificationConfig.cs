using System;

namespace NetworkDeviceMonitor.Lib.Config
{
    public interface INotificationConfig
    {
        int LowBatteryPercentage { get; }
        int LowNetworkBPS { get; }

        TimeSpan StatusCheckInterval { get; }
    }
}