using NetworkDeviceMonitor.Lib.Config;
using NetworkDeviceMonitor.Lib.DAL;
using NetworkDeviceMonitor.Lib.Model;
using NetworkDeviceMonitor.Lib.Model.DataStore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NetworkDeviceMonitor.Lib.Logic
{
    public class JioDeviceMonitorLogic : IJioDeviceMonitorLogic, IDisposable
    {
        /// <summary>
        /// Http client for communicating to the local jio api.
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// Serializer to deserialize the jio response.
        /// </summary>
        private readonly XmlSerializer responseSerializer;

        /// <summary>
        /// Configuration having the expected notification settings.
        /// </summary>
        private readonly INotificationConfig config;

        /// <summary>
        /// Timer to periodically check the device status.
        /// </summary>
        private readonly Timer timer;
        private readonly NetworkDataStore networkDataStore;
        const string jioDeviceStatusUrlFormat = "http://jiofi.local.html/st_dev.w.xml?_={0}";

        /// <summary>
        /// Battery level at the time the charging starts.
        /// </summary>
        private int lastChargeStartsAtBatteryLevel = 0;

        /// <summary>
        /// Time when the charging starts.
        /// </summary>
        private DateTime lastChargeStartsAt = DateTime.MinValue;

        /// <summary>
        /// Time when the charging ended.
        /// </summary>
        private DateTime lastChargeEndsAt = DateTime.MinValue;

        /// <summary>
        /// Flag indicating the battery charging status.
        /// </summary>
        private bool isBatteryCharging = false;

        /// <summary>
        /// Event raised on notifications.
        /// </summary>
        public OnNotificationsEvent NotificationEvent { get; set; }


        /// <summary>
        /// Constructor - initializes the JioDeviceMonitorLogic instance.
        /// </summary>
        /// <param name="httpClient">Http client to talk to jio API.</param>
        /// <param name="config">Notification settings.</param>
        public JioDeviceMonitorLogic(HttpClient httpClient, INotificationConfig config)
        {
            this.httpClient = httpClient;
            this.responseSerializer = new XmlSerializer(typeof(StDevResponseRoot));
            this.config = config;
            this.timer = new Timer(StatusCheckTimer_OnTimeoutAsync, null, Timeout.Infinite, -1);
            this.networkDataStore = new NetworkDataStore("Data Source=" + Path.Combine(Directory.GetCurrentDirectory(), "networkdevice.db"));
        }

        /// <summary>
        /// Timer timeout event handler.
        /// </summary>
        /// <param name="state"></param>
        private async void StatusCheckTimer_OnTimeoutAsync(object state)
        {
            var notifications = await GetNotificationsAsync();
            if (notifications != null
                && NotificationEvent != null)
            {
                try
                {
                    NotificationEvent(notifications);
                }
                catch { /*tbd*/}
            }
        }

        public void StarDeviceMonitor()
        {
            if (NotificationEvent != null)
                this.timer.Change(TimeSpan.Zero, this.config.StatusCheckInterval);
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

        /// <summary>
        /// Gets the notifications and information about the network device.
        /// </summary>
        /// <returns>Notifications and information about the network device</returns>
        public async Task<NotificationModel> GetNotificationsAsync()
        {
            var deviceStatus = await this.GetDeviceDetailsAsync();
            Dictionary<string, string> notifications;
            Dictionary<string, string> info;
            int batteryPercentage = 0;

            BatteryStatus batteryStatus = BatteryStatus.NoBattery;

            if (deviceStatus != null)
            {
                batteryStatus = ParseBatteryStatus(deviceStatus.Batt_st);
                batteryPercentage = deviceStatus.BatteryPercentage;

                notifications = FormNotifications(deviceStatus, batteryStatus);
                info = FormInformation(deviceStatus, batteryStatus);

                try
                {
                    var batteryInfo = new BatteryDataItem()
                    {
                        BatteryPercentage = deviceStatus.BatteryPercentage,
                        EventDateTime = DateTime.Now,
                        IsCharging = batteryStatus == BatteryStatus.Charging
                    };

                    await this.networkDataStore.SaveAsync(batteryInfo);
                }
                catch (Exception ex)
                {
                    // have to handle exception
                }
            }
            else
            {
                notifications = new Dictionary<string, string>();
                info = new Dictionary<string, string>();
            }

            //try
            //{
            //    var allRecords = await networkDataStore.GetAsync(DateTime.MinValue, DateTime.Now);
            //}
            //catch (Exception exp)
            //{
            //    //tbd
            //}
            var notificationModel = new NotificationModel(
                notifications,
                batteryStatus == BatteryStatus.Charging,
                info,
                batteryPercentage);

            notificationModel.IsOnLowBattery = IsLowBattery(batteryPercentage, batteryStatus);

            return notificationModel;
        }

        private Dictionary<string, string> FormInformation(StDevResponseRoot deviceStatus, BatteryStatus batteryStatus)
        {
            Dictionary<string, string> info = new Dictionary<string, string>();

            info.Add("Device phone#", deviceStatus.Msisdn);
            info.Add("Battery status", Enum.GetName(typeof(BatteryStatus), batteryStatus));
            info.Add("Battery %", deviceStatus.BatteryPercentage.ToString());

            if (!isBatteryCharging && batteryStatus == BatteryStatus.Charging)
            {
                isBatteryCharging = true;
                lastChargeStartsAt = DateTime.Now;
                lastChargeStartsAtBatteryLevel = deviceStatus.BatteryPercentage;
                info.Add("Charge Time (h:m:s)", FormChargingTime(true, lastChargeStartsAt, DateTime.Now/*It does not matter*/));
                info.Add("Charging since battery %", lastChargeStartsAtBatteryLevel.ToString());
            }
            else if (isBatteryCharging
                && batteryStatus != BatteryStatus.Unknown
                && batteryStatus != BatteryStatus.Charging)
            {
                isBatteryCharging = false;
                lastChargeEndsAt = DateTime.Now;
                info.Add("Charge Time (h:m:s)", FormChargingTime(false, lastChargeStartsAt, lastChargeEndsAt));
            }
            else if (isBatteryCharging)
            {
                info.Add("Charge Time (h:m:s)", FormChargingTime(true, lastChargeStartsAt, DateTime.Now/*It does not matter*/));
                info.Add("Charging since battery %", lastChargeStartsAtBatteryLevel.ToString());
            }

            return info;
        }

        private Dictionary<string, string> FormNotifications(StDevResponseRoot deviceStatus, BatteryStatus batteryStatus)
        {
            Dictionary<string, string> notifications = new Dictionary<string, string>();

            if (IsLowBattery(deviceStatus.BatteryPercentage, batteryStatus))
            {
                notifications.Add("Network device is on Low battery. ", deviceStatus.BatteryPercentage.ToString() + "%");
            }

            if (batteryStatus == BatteryStatus.FullyCharged)
            {
                notifications.Add("Battery is full. Disconnect charger.", deviceStatus.BatteryPercentage.ToString() + "%");
            }

            return notifications;
        }

        private bool IsLowBattery(int batteryPercentage, BatteryStatus batteryStatus)
        {
            return batteryPercentage != -100 
                && batteryPercentage < this.config.LowBatteryPercentage
                && batteryStatus == BatteryStatus.Discharging;
        }

        /// <summary>
        /// Forms the given time displayable string.
        /// </summary>
        /// <param name="isCharging">Flag indicating the charging status.</param>
        /// <param name="chargeStartsAt">DateTime when charging started.</param>
        /// <param name="chargeEndsAt">DateTime when charging ended. Pass if the battery is not charging.</param>
        /// <returns>Formatted date time string.</returns>
        private static string FormChargingTime(bool isCharging, DateTime chargeStartsAt, DateTime chargeEndsAt)
        {
            if (isCharging)
            {
                var chargeTime = DateTime.Now.Subtract(chargeStartsAt);
                return chargeTime.ToString(@"hh\:mm\:ss");
            }

            return chargeEndsAt.Subtract(chargeStartsAt).ToString(@"hh\:mm\:ss");
        }

        /// <summary>
        /// Utility method to parse and get us the battery status.
        /// This logic is taken from jio api web page.
        /// </summary>
        /// <param name="batteryStatus"></param>
        /// <returns>The battery status enum.</returns>
        /// <seealso cref="BatteryStatus"/>
        private static BatteryStatus ParseBatteryStatus(int batteryStatus)
        {
            /*
             * Jio device's battery status logic extracted from http://jiofi.local.html.
             * Do not consider if the battery status value is < 1.
             */
            if (batteryStatus < 1)
            {
                return BatteryStatus.Unknown;
            }

            uint rightShifted = (uint)batteryStatus >> 8;
            if (rightShifted < 4)
                return BatteryStatus.Discharging;
            else if (rightShifted == 4)
                return BatteryStatus.Charging;
            else if (rightShifted == 5)
                return BatteryStatus.FullyCharged;

            return BatteryStatus.NoBattery;
        }

        /// <summary>
        /// Cleanup the unmanaged http client, stop the timer, etc.
        /// </summary>
        public void Dispose()
        {
            this.StopDeviceMonitor();
            this.httpClient.Dispose();
        }
    }
}
