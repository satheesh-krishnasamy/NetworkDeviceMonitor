using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkDeviceMonitor.Lib.Model
{
    public sealed class NotificationModel
    {
        public NotificationModel(
            Dictionary<string, string> noticiations,
            bool isCharging = false,
            IReadOnlyDictionary<string, string> information = null)
        {
            this.Notifications = noticiations;
            this.IsCharging = isCharging;
            this.Information = information;
        }

        public bool IsCharging { get; private set; }

        public IReadOnlyDictionary<string, string> Notifications { get; private set; }

        public IReadOnlyDictionary<string, string> Information { get; private set; }

    }
}
