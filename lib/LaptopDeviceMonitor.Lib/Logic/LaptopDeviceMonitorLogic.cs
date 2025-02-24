using DeviceMonitor.Lib.Shared.Logic;
using WorkStationAssistant.Lib.Config;
using WorkStationAssistant.Lib.Model;
using WorkStationAssistant.Lib.Model.DataStore;

namespace WorkStationAssistant.Lib.Logic
{
    public class LaptopDeviceMonitorLogic : DeviceMonitorLogicBase, IDeviceMonitorLogic
    {
        public DeviceType DeviceType => DeviceType.Laptop;

        /// <summary>
        /// Constructor - initializes the LaptopDeviceMonitorLogic instance.
        /// </summary>
        /// <param name="config">Notification settings.</param>
        /// 
        public LaptopDeviceMonitorLogic(INotificationConfig config)
            : base($"laptopdevice_{config.DbVersion}.db")
        {
        }


        private async Task<PowerStatus> GetDeviceDetailsAsync()
        {
            try
            {
                PowerStatus pwr = SystemInformation.PowerStatus;
                return pwr;
            }
            catch
            {
                // TBD: error handling.
                return null;
            }
        }

        public async Task Init()
        {
            var deviceStatus = await this.GetDeviceDetailsAsync();
            if ((deviceStatus != null && IsCharging(deviceStatus)))
            {
                await base.InititializeLastSession(true);
            }
            else
            {
                await base.InititializeLastSession(false);
            }
        }

        /// <summary>
        /// Gets the notifications and information about the network device.
        /// </summary>
        /// <returns>Notifications and information about the network device</returns>
        public async Task<NotificationModel> GetNotificationsAsync()
        {
            var deviceStatus = await this.GetDeviceDetailsAsync();
            IList<NotificationMessage> notifications = new List<NotificationMessage>();
            int batteryPercentage = -1;

            if (deviceStatus != null)
            {
                batteryPercentage = GetBatteryPercentage(deviceStatus);
                CheckAndAddBatteryAlerts(
                    deviceStatus.PowerLineStatus == PowerLineStatus.Online,
                    deviceStatus.PowerLineStatus == PowerLineStatus.Unknown,
                    notifications,
                    batteryPercentage,
                    DeviceType.Laptop);

                await SaveLaptopBatteryInfoToDb(deviceStatus);
            }

            FormInformation(deviceStatus, notifications);

            var notificationModel = new NotificationModel(
                notifications,
                deviceStatus.BatteryChargeStatus == BatteryChargeStatus.Charging,
                batteryPercentage);

            notificationModel.DeviceType = this.DeviceType;
            notificationModel.IsOnLowBattery = batteryPercentage < 20;

            return notificationModel;
        }

        private async Task SaveLaptopBatteryInfoToDb(PowerStatus? deviceStatus)
        {
            try
            {
                var batteryInfo = new BatteryDataItem()
                {
                    BatteryPercentage = GetBatteryPercentage(deviceStatus),
                    EventDateTime = DateTime.Now,
                    IsCharging = IsCharging(deviceStatus)
                };

                await base.SaveAsync(batteryInfo);
            }
            catch (Exception ex)
            {
                // have to handle exception
            }
        }

        private static int GetBatteryPercentage(PowerStatus? deviceStatus)
        {
            return (int)(deviceStatus.BatteryLifePercent * 100.0);
        }

        private bool IsCharging(PowerStatus? deviceStatus)
        {
            return deviceStatus != null && deviceStatus.PowerLineStatus == PowerLineStatus.Online;
        }

        private void FormInformation(
            PowerStatus deviceStatus,
            IList<NotificationMessage> info)
        {
            bool isCharging = IsCharging(deviceStatus);
            AddInformation(GetBatteryPercentage(deviceStatus), isCharging, info);
        }
    }
}
