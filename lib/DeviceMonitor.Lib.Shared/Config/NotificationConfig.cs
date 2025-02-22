using Microsoft.Extensions.Configuration;

namespace WorkStationAssistant.Lib.Config
{
    public sealed class NotificationConfig : INotificationConfig
    {

        public NotificationConfig(IConfiguration config)
        {
            this.LowBatteryPercentage = ParseToInt(config, "Notification:LowBatteryNotificationPercentage", 10, 10);
            this.LowNetworkBPS = ParseToInt(config, "Notification:LowNetworkBPSNotificationThreshold", 64 * 1024, 64000);
            this.StatusCheckInterval = TimeSpan.FromSeconds(ParseToInt(config, "Notification:StatusCheckIntervalInSeconds", 30, 10));
            this.DbVersion = config.GetSection("DbVersion").Value;
        }

        private static int ParseToInt(IConfiguration config, string fullSectionPath, int defaultValueIfMissing, int minExpectedValue)
        {
            int tempValueConvertedToInt;
            var configSection = config.GetSection(fullSectionPath);
            if (!configSection.Exists()
                || !int.TryParse(configSection.Value, out tempValueConvertedToInt))
            {
                return defaultValueIfMissing;
            }

            if (tempValueConvertedToInt < minExpectedValue)
            {
                tempValueConvertedToInt = minExpectedValue;
            }

            return tempValueConvertedToInt;
        }

        public int LowBatteryPercentage { get; private set; }

        public int LowNetworkBPS { get; private set; }

        public TimeSpan StatusCheckInterval { get; private set; }
        public string? DbVersion { get; private set; }
    }
}
