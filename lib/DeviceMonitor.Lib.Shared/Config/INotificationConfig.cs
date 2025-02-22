namespace WorkStationAssistant.Lib.Config
{
    public interface INotificationConfig
    {
        int LowBatteryPercentage { get; }
        int LowNetworkBPS { get; }

        TimeSpan StatusCheckInterval { get; }

        string DbVersion { get; }
    }
}