﻿using Microsoft.Extensions.Logging;
using WorkStationAssistant.Lib.DAL;
using WorkStationAssistant.Lib.Model;
using WorkStationAssistant.Lib.Model.DataStore;
using System.Text;
using DeviceMonitor.Lib.Shared.Logic;

namespace WorkStationAssistant.Lib.Logic
{
    public abstract class DeviceMonitorLogicBase
    {
        private readonly IDeviceMonitorDataStore dataStore;

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

        private string chargingSessionId = string.Empty;

        private BatteryDataItem lastBatteryItem = null;

        protected DeviceMonitorLogicBase(string storeName, DeviceType deviceType)
        {
            this.dataStore = new DeviceMonitorDataStore("Data Source=" + Path.Combine(Directory.GetCurrentDirectory(), storeName));
            DeviceType = deviceType;
        }

        public DeviceType DeviceType { get; protected set; }

        protected async Task InititializeLastSession(bool isCharging)
        {
            if (!isCharging)
            {
                isBatteryCharging = false;
                lastChargeStartsAtBatteryLevel = 0;
                lastChargeStartsAt = DateTime.MinValue;
                lastChargeEndsAt = DateTime.MinValue;
                chargingSessionId = string.Empty;
                return;
            }

            var startTimeToDetectLastChargingSession = DateTime.Now.AddMinutes(-15);
            var lastChargeSessionRecords = await this.dataStore.GetLastBatteryChargingSessionsAsync(
                startTimeToDetectLastChargingSession,
                DateTime.Now).ConfigureAwait(false);

            if (lastChargeSessionRecords != null && lastChargeSessionRecords.Any())
            {
                var lastChargeRecord = lastChargeSessionRecords.FirstOrDefault();
                if (lastChargeRecord != null)
                {
                    isBatteryCharging = true;
                    lastChargeStartsAt = lastChargeRecord.EventDateTime;
                    lastChargeStartsAtBatteryLevel = lastChargeRecord.BatteryPercentage;
                    lastChargeEndsAt = DateTime.MinValue;
                    chargingSessionId = lastChargeRecord.ChargingSessionId ?? string.Empty;
                }
            }
        }

        protected static void CheckAndAddBatteryAlerts(
            bool isCharging,
            bool noBatteryConnected,
            IList<NotificationMessage> notifications,
            int batteryPercentage,
            DeviceType deviceType)
        {
            if (noBatteryConnected)
                return;

            if (!isCharging)
            {
                if (batteryPercentage <= 10)
                {
                    notifications.Add(
                        new NotificationMessage()
                        {
                            Title = $"{deviceType} is on Low battery. ",
                            Message = batteryPercentage + "%",
                            Severity = LogLevel.Critical,
                            RequiresAttention = true
                        });
                }
                else if (batteryPercentage < 20)
                {
                    notifications.Add(
                        new NotificationMessage()
                        {
                            Title = $"{deviceType} is on Low battery. ",
                            Message = batteryPercentage + "%",
                            Severity = LogLevel.Warning,
                            RequiresAttention = true
                        });
                }
            }
            else if (!noBatteryConnected) // not charging and the battery is connected
            {
                if (batteryPercentage >= 90)
                {
                    notifications.Add(
                        new NotificationMessage()
                        {
                            Title = "Charging can be stopped for better battery life. ",
                            Message = batteryPercentage + "%",
                            Severity = LogLevel.Critical,
                            RequiresAttention = true
                        });
                }
                else if (batteryPercentage >= 80)
                {
                    notifications.Add(
                        new NotificationMessage()
                        {
                            Title = $"{deviceType} - Charging can be stopped for better battery life. ",
                            Message = batteryPercentage + "%",
                            Severity = LogLevel.Warning
                        });
                }
                else if (batteryPercentage >= 100)
                {
                    notifications.Add(
                        new NotificationMessage()
                        {
                            Title = $"{deviceType} - Stop charging. Battery is full.",
                            Message = batteryPercentage + "%",
                            Severity = LogLevel.Critical,
                            RequiresAttention = true
                        });
                }
            }
        }

        public async Task<string> GetChartDataAsync()
        {
            var minUTCDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var batteryChargeHostoryData = await dataStore
                .GetAsync(DateTime.Now.AddHours(-6), DateTime.Now);
            StringBuilder dataString = new StringBuilder();

            foreach (var d in batteryChargeHostoryData)
            {
                if (d.BatteryPercentage < 0)
                    continue;

                dataString.AppendFormat("[{0}, {1}],",
                    new DateTimeOffset(d.EventDateTime).ToUnixTimeMilliseconds(),
                    d.BatteryPercentage);
            }
            return "var chartData = [" +
                (dataString.Length > 0 ? dataString.Remove(dataString.Length - 1, 1).ToString() : "")
                + "];";
        }

        protected void AddInformation(int batteryPercentage, bool isCharging, IList<NotificationMessage> info)
        {
            BatteryStatus batteryStatus = isCharging ? BatteryStatus.Charging : BatteryStatus.Discharging;

            info.Add(new NotificationMessage()
            {
                Title = "Battery status",
                Message = isCharging ? "Charging" : "Not charging",
                Severity = LogLevel.Information
            });
            info.Add(new NotificationMessage()
            {
                Title = "Battery %",
                Message = batteryPercentage.ToString(),
                Severity = LogLevel.Information
            });

            if (IsBatteryChargingSessionStartedNewly(batteryStatus))
            {
                UpdateBatteryChargingStarted(batteryPercentage);

                info.Add(new NotificationMessage()
                {
                    Title = "Charge Time (h:m:s)",
                    Message = FormChargingTime(true, lastChargeStartsAt, DateTime.Now/*It does not matter*/)
                });

                info.Add(new NotificationMessage()
                {
                    Title = "Charging since battery %",
                    Message = lastChargeStartsAtBatteryLevel.ToString()
                });
            }
            else if (IsBatteryChargingSessionCompleted(batteryStatus))
            {
                UpdateBatteryChargingCompleted(batteryPercentage);

                info.Add(new NotificationMessage()
                {
                    Title = "Charge Time (h:m:s)",
                    Message = FormChargingTime(false, lastChargeStartsAt, lastChargeEndsAt)
                });
            }
            else if (isBatteryCharging)
            {
                info.Add(new NotificationMessage()
                {
                    Title = "Charge Time (h:m:s)",
                    Message = FormChargingTime(true, lastChargeStartsAt, DateTime.Now)
                });

                info.Add(new NotificationMessage()
                {
                    Title = "Charging since battery %",
                    Message = lastChargeStartsAtBatteryLevel.ToString()
                });
            }
        }

        private bool IsBatteryChargingSessionCompleted(BatteryStatus batteryStatus)
        {
            if (!isBatteryCharging)
                return false;

            lock (lockObj)
            {
                return isBatteryCharging
                            && (batteryStatus == BatteryStatus.Discharging
                            || batteryStatus == BatteryStatus.NoBattery
                            || batteryStatus == BatteryStatus.Unknown);
            }
        }

        private bool IsBatteryChargingSessionStartedNewly(BatteryStatus batteryStatus)
        {
            if (isBatteryCharging)
                return false;

            lock (lockObj)
            {
                return !isBatteryCharging && batteryStatus == BatteryStatus.Charging;
            }
        }

        private object lockObj = new object();

        private void UpdateBatteryChargingCompleted(int batteryPercentage)
        {
            UpdateBatteryStatus(true, false, batteryPercentage);
        }

        private void UpdateBatteryChargingStarted(int batteryPercentage)
        {
            UpdateBatteryStatus(false, true, batteryPercentage);
        }

        private void UpdateBatteryStatus(bool currentValue, bool batteryCharingNew, int batteryPercentage)
        {
            lock (lockObj)
            {
                if (this.isBatteryCharging == currentValue)
                {
                    this.isBatteryCharging = batteryCharingNew;

                    if (!batteryCharingNew)
                    {
                        isBatteryCharging = false;
                        lastChargeEndsAt = DateTime.Now;
                        chargingSessionId = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        isBatteryCharging = true;
                        lastChargeStartsAt = DateTime.Now;
                        lastChargeStartsAtBatteryLevel = batteryPercentage;
                        chargingSessionId = Guid.NewGuid().ToString(); // new charge session
                    }
                }
            }
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

        public async Task<bool> SaveAsync(BatteryDataItem batteryInfo)
        {
            if (batteryInfo == null)
                return false;

            if (ShouldSaveData(batteryInfo))
            {
                lastBatteryItem = batteryInfo;
                batteryInfo.ChargingSessionId = chargingSessionId;
                return await this.dataStore.SaveAsync(batteryInfo);
            }

            return false;
        }

        private bool ShouldSaveData(BatteryDataItem batteryInfo)
        {
            return lastBatteryItem == null
                            || batteryInfo.IsCharging != lastBatteryItem.IsCharging
                            || batteryInfo.BatteryPercentage != lastBatteryItem.BatteryPercentage
                            || (batteryInfo.IsCharging && batteryInfo.BatteryPercentage == 100);
        }

        public async Task<Dictionary<string, string>> GetStatisticsAsync(DateTime start, DateTime end)
        {
            var d = new Dictionary<string, string>();
            var dbResult = await this.dataStore.GetBatteryStatisticsAsync(start, end);
            if (dbResult != null && dbResult.Any())
            {
                var totalChargeCycles = dbResult.GroupBy(c => c.EndedDateTime.Date);
                foreach (var chargeCycle in totalChargeCycles)
                {
                    d.Add($"{this.DeviceType} - Total charges on [{chargeCycle.Key}]", chargeCycle?.Count().ToString());
                }
            }

            return d;
        }
    }
}