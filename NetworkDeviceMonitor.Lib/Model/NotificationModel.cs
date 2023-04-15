using System;
using System.Collections.Generic;

namespace NetworkDeviceMonitor.Lib.Model
{
    public sealed class NotificationModel
    {
        public NotificationModel(
            Dictionary<string, string> noticiations,
            bool isCharging = false,
            IReadOnlyDictionary<string, string> information = null,
            int batteryPercentage = 0)
        {
            this.Notifications = noticiations;
            this.IsCharging = isCharging;
            this.Information = information;
            this.LastStatusCheckOn = DateTime.Now;
            BatteryPercentage = batteryPercentage;
        }

        public bool IsCharging { get; private set; }

        public IReadOnlyDictionary<string, string> Notifications { get; private set; }

        public IReadOnlyDictionary<string, string> Information { get; private set; }

        public DateTime LastStatusCheckOn { get; private set; }

        public int BatteryPercentage { get; set; }

        public bool IsOnLowBattery { get; set; }

    }
}
