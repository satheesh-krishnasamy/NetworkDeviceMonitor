using DeviceMonitor.Lib.Shared.Logic;

namespace WorkStationAssistant.Lib.Model
{
    public interface INotificationModel
    {
        int BatteryPercentage { get; set; }
        DeviceType DeviceType { get; set; }
        IReadOnlyDictionary<string, string> Information { get; }
        bool IsCharging { get; }
        bool IsOnLowBattery { get; set; }
        DateTime LastStatusCheckOn { get; }
        IEnumerable<NotificationMessage> Messages { get; }
    }
}