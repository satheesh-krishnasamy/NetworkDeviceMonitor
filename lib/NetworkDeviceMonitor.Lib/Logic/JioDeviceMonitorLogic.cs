using DeviceMonitor.Lib.Shared.Logic;
using WorkStationAssistant.Lib.Config;
using WorkStationAssistant.Lib.Model;
using WorkStationAssistant.Lib.Model.DataStore;
using System.Xml.Serialization;

namespace WorkStationAssistant.Lib.Logic
{
    public class JioDeviceMonitorLogic : DeviceMonitorLogicBase, IDeviceMonitorLogic
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

        const string jioDeviceStatusUrlFormat = "http://jiofi.local.html/st_dev.w.xml?_={0}";

        public DeviceType DeviceType => DeviceType.NetworkDevice;


        /// <summary>
        /// Constructor - initializes the JioDeviceMonitorLogic instance.
        /// </summary>
        /// <param name="httpClient">Http client to talk to jio API.</param>
        /// <param name="config">Notification settings.</param>
        public JioDeviceMonitorLogic(HttpClient httpClient, INotificationConfig config)
            : base($"networkdevice_{config.DbVersion}.db")
        {
            this.httpClient = httpClient;
            this.responseSerializer = new XmlSerializer(typeof(StDevResponseRoot));
            this.config = config;
        }

        public async Task Init()
        {
            var deviceStatus = await this.GetDeviceDetailsAsync();
            if ((deviceStatus != null))
            {
                var batteryStatus = ParseBatteryStatus(deviceStatus.Batt_st);
                var isCharging = IsLowBattery(deviceStatus.BatteryPercentage, batteryStatus);

                await base.InititializeLastSession(isCharging);
            }
            else
            {
                await base.InititializeLastSession(false);
            }
        }

        private async Task<StDevResponseRoot> GetDeviceDetailsAsync()
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
            List<NotificationMessage> notifications = new List<NotificationMessage>();
            int batteryPercentage = 0;

            BatteryStatus batteryStatus = BatteryStatus.NoBattery;

            if (deviceStatus != null)
            {
                batteryStatus = ParseBatteryStatus(deviceStatus.Batt_st);
                batteryPercentage = deviceStatus.BatteryPercentage;

                notifications = FormNotifications(deviceStatus, batteryStatus);
                FormInformation(deviceStatus, batteryStatus, notifications);

                try
                {
                    var batteryInfo = new BatteryDataItem()
                    {
                        BatteryPercentage = deviceStatus.BatteryPercentage,
                        EventDateTime = DateTime.Now,
                        IsCharging = batteryStatus == BatteryStatus.Charging                    };

                    await base.SaveAsync(batteryInfo);
                }
                catch (Exception ex)
                {
                    // have to handle exception
                }
            }

            var notificationModel = new NotificationModel(
                notifications,
                batteryStatus == BatteryStatus.Charging,
                batteryPercentage);

            notificationModel.DeviceType = this.DeviceType;
            notificationModel.IsOnLowBattery = IsLowBattery(batteryPercentage, batteryStatus);

            return notificationModel;
        }

        private void FormInformation(
            StDevResponseRoot deviceStatus,
            BatteryStatus batteryStatus,
            List<NotificationMessage> info)
        {
            info.Add(new NotificationMessage("Device phone#", deviceStatus.Msisdn));
            // info.Add(new NotificationMessage("Battery status", Enum.GetName(typeof(BatteryStatus), batteryStatus)));
            // info.Add(new NotificationMessage("Battery %", deviceStatus.BatteryPercentage.ToString()));

            AddInformation(deviceStatus.BatteryPercentage,
                batteryStatus == BatteryStatus.Charging,
                info);

            //if (!isBatteryCharging && batteryStatus == BatteryStatus.Charging)
            //{
            //    isBatteryCharging = true;
            //    lastChargeStartsAt = DateTime.Now;
            //    lastChargeStartsAtBatteryLevel = deviceStatus.BatteryPercentage;
            //    info.Add(new NotificationMessage("Charge Time (h:m:s)",
            //        FormChargingTime(true, lastChargeStartsAt, DateTime.Now/*It does not matter*/)));
            //    info.Add(new NotificationMessage("Charging since battery %", lastChargeStartsAtBatteryLevel.ToString()));
            //}
            //else if (isBatteryCharging
            //    && batteryStatus != BatteryStatus.Unknown
            //    && batteryStatus != BatteryStatus.Charging)
            //{
            //    isBatteryCharging = false;
            //    lastChargeEndsAt = DateTime.Now;
            //    info.Add(new NotificationMessage("Charge Time (h:m:s)",
            //        FormChargingTime(false, lastChargeStartsAt, lastChargeEndsAt)));
            //}
            //else if (isBatteryCharging)
            //{
            //    info.Add(new NotificationMessage("Charge Time (h:m:s)",
            //        FormChargingTime(true, lastChargeStartsAt, DateTime.Now/*It does not matter*/)));
            //    info.Add(new NotificationMessage("Charging since battery %", lastChargeStartsAtBatteryLevel.ToString()));
            //}
        }

        private List<NotificationMessage> FormNotifications(
            StDevResponseRoot deviceStatus,
            BatteryStatus batteryStatus)
        {
            List<NotificationMessage> notifications = new List<NotificationMessage>();
            var isCharging = IsLowBattery(deviceStatus.BatteryPercentage, batteryStatus);

            CheckAndAddBatteryAlerts(
                    isCharging,
                    deviceStatus.BatteryPercentage == -100 || deviceStatus.BatteryPercentage < 0,
                    notifications,
                    deviceStatus.BatteryPercentage);

            return notifications;
        }

        private bool IsLowBattery(int batteryPercentage, BatteryStatus batteryStatus)
        {
            return batteryPercentage != -1
                && batteryPercentage != -100
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
    }
}
