using NetworkDeviceMonitor.Lib.Config;
using NetworkDeviceMonitor.Lib.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NetworkDeviceMonitor.Lib.Logic
{


    public class JioDeviceMonitorLogic : IJioDeviceMonitorLogic, IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly XmlSerializer responseSerializer;
        private readonly INotificationConfig config;
        private readonly Timer timer;
        const string jioDeviceStatusUrlFormat = "http://jiofi.local.html/st_dev.w.xml?_={0}";

        public OnNotificationsEvent NotificationEvent { get; set; }

        public JioDeviceMonitorLogic(HttpClient httpClient, INotificationConfig config)
        {
            this.httpClient = httpClient;
            this.responseSerializer = new XmlSerializer(typeof(StDevResponseRoot));
            this.config = config;
            this.timer = new Timer(StatusCheckTimer_OnTimeoutAsync, null, Timeout.Infinite, -1);
        }

        private async void StatusCheckTimer_OnTimeoutAsync(object state)
        {
            var notifications = GetNotifications().GetAwaiter().GetResult();
            if (notifications != null
                && NotificationEvent != null)
            {
                NotificationEvent(notifications);
            }

            StarDeviceMonitor();
        }

        public void StarDeviceMonitor()
        {
            if (NotificationEvent != null)
                this.timer.Change(0, this.config.StatusCheckInterval.Milliseconds);
        }

        public void StopDeviceMonitor()
        {
            try
            {
                this.timer.Change(0, -1);
            }
            catch { }
        }

        public async Task<StDevResponseRoot> GetDeviceDetailsAsync()
        {
            try
            {
                var httpResponseXml = await httpClient.GetAsync(
                    string.Format(jioDeviceStatusUrlFormat, DateTime.UnixEpoch.ToString()));

                var responseXml = await httpResponseXml.Content.ReadAsStreamAsync();

                return (StDevResponseRoot)this.responseSerializer.Deserialize(responseXml);
            }
            catch
            {
                // TBD: error handling.
                return null;
            }
        }

        public async Task<NotificationModel> GetNotifications()
        {
            var deviceStatus = await this.GetDeviceDetailsAsync();
            var notifications = new Dictionary<string, string>();
            BatteryStatus batteryStatus = BatteryStatus.NoBattery;
            var info = new Dictionary<string, string>();


            if (deviceStatus != null)
            {
                batteryStatus = ParseBatteryStatus(deviceStatus.Batt_st);
                if (deviceStatus.BatteryPercentage < this.config.LowBatteryPercentage
                    && batteryStatus == BatteryStatus.Discharging)
                {
                    notifications.Add("Network device is on Low battery. ", deviceStatus.BatteryPercentage.ToString() + "%");
                }

                if (batteryStatus == BatteryStatus.FullyCharged)
                {
                    notifications.Add("Battery is full. Disconnect charger.", deviceStatus.BatteryPercentage.ToString() + "%");
                }

                info.Add("Device phone#", deviceStatus.Msisdn);
                info.Add("Battery status", Enum.GetName(typeof(BatteryStatus), batteryStatus));
                info.Add("Battery %", deviceStatus.BatteryPercentage.ToString());
            }

            return new NotificationModel(notifications, batteryStatus == BatteryStatus.Charging, info);
        }

        private BatteryStatus ParseBatteryStatus(int batteryStatus)
        {
            uint rightShifted = (uint)batteryStatus >> 8;
            if (rightShifted < 4)
                return BatteryStatus.Discharging;
            else if (rightShifted == 4)
                return BatteryStatus.Charging;
            else if (rightShifted == 5)
                return BatteryStatus.FullyCharged;

            return BatteryStatus.NoBattery;
        }

        public void Dispose()
        {
            this.StopDeviceMonitor();
            this.httpClient.Dispose();
        }
    }

    public enum BatteryStatus
    {
        NoBattery = 0,
        FullyCharged = 1,
        Charging = 2,
        Discharging = 3
    }
}
