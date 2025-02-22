using DeviceMonitor.Lib.Shared.Logic;
using Microsoft.Extensions.Logging;

namespace WorkStationAssistant.Lib.Model
{
    public sealed class NotificationModel
    {

        public NotificationModel(
            IList<NotificationMessage> noticiations,
            bool isCharging = false,
            int batteryPercentage = 0)
        {
            this.IsCharging = isCharging;
            this.LastStatusCheckOn = DateTime.Now;
            BatteryPercentage = batteryPercentage;
            Messages = noticiations;
        }

        public bool IsCharging { get; private set; }

        public DeviceType DeviceType { get; set; }

        // public IReadOnlyDictionary<string, string> Notifications { get; private set; }

        public IEnumerable<NotificationMessage> Messages { get; private set; }

        public DateTime LastStatusCheckOn { get; private set; }

        public int BatteryPercentage { get; set; }

        public bool IsOnLowBattery { get; set; }

    }

    public sealed class NotificationMessage
    {

        public NotificationMessage() { }

        public NotificationMessage(string title, string msg)
        {
            Title = title;
            Message = msg;
        }

        public NotificationMessage(string title, string msg, LogLevel severity)
        {
            Title = title;
            Message = msg;
            Severity = severity;
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public LogLevel Severity { get; set; } = LogLevel.Information;

        public bool RequiresAttention { get; set; }
    }
}
