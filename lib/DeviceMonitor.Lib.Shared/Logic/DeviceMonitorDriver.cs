using WorkStationAssistant.Lib.Config;
using WorkStationAssistant.Lib.Model;

namespace DeviceMonitor.Lib.Shared.Logic
{
    public sealed class DeviceMonitorDriver : IDisposable
    {
        /// <summary>
        /// Timer to periodically check the device status.
        /// </summary>
        private readonly Timer timer;

        /// <summary>
        /// Configuration having the expected notification settings.
        /// </summary>
        private readonly INotificationConfig config;

        private readonly IEnumerable<IDeviceMonitorLogic> deviceMonitors;

        public OnNotificationsEvent NotificationEvent { get; set; }


        public DeviceMonitorDriver(
            IEnumerable<IDeviceMonitorLogic> deviceMonitors,
            INotificationConfig config)
        {
            ArgumentNullException.ThrowIfNull(deviceMonitors);
            ArgumentNullException.ThrowIfNull(config);

            if (!deviceMonitors.Any())
                throw new ArgumentException("No device monitors are provided.");


            this.config = config;
            this.deviceMonitors = deviceMonitors;

            foreach (var deviceMonitor in deviceMonitors)
                deviceMonitor.Init();

            this.timer = new Timer(StatusCheckTimer_OnTimeoutAsync, null, Timeout.Infinite, -1);
        }

        /// <summary>
        /// Timer timeout event handler.
        /// </summary>
        /// <param name="state"></param>
        private async void StatusCheckTimer_OnTimeoutAsync(object? state)
        {
            IEnumerable<NotificationModel> notifications = await GetNotificationsAsync().ConfigureAwait(false);

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

        public async Task<string> GetChartDataAsync(DeviceType deviceType)
        {
            var dm = this.deviceMonitors.First(d => d.DeviceType == deviceType);
            return await dm.GetChartDataAsync();
        }

        public async Task<IEnumerable<NotificationModel>> GetNotificationsAsync()
        {
            IList<Task<NotificationModel>> tasks = new List<Task<NotificationModel>>();

            foreach (var d in deviceMonitors)
            {
                tasks.Add(d.GetNotificationsAsync());
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
            var notifications = tasks.Select(t => t.Result);

            return notifications;
        }

        public async Task<Dictionary<string, string>> GetStatisticsAsync()
        {
            IList<Task<Dictionary<string, string>>> tasks = new List<Task<Dictionary<string, string>>>();
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (var d in deviceMonitors)
            {
                tasks.Add(d.GetStatisticsAsync(DateTime.Now.Date, DateTime.Now));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
            var notifications = tasks.Select(t => t.Result);

            foreach (var d in notifications)
                foreach (var kv in d)
                    result[kv.Key] = kv.Value;

            return result;
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

        public void Dispose()
        {
            this.timer?.Dispose();
        }
    }
}
