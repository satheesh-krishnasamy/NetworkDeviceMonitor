using DeviceMonitor.Lib.Dal.DataStore;
using WorkStationAssistant.Lib.Model;

namespace DeviceMonitor.Lib.Shared.Logic
{
    public interface IDeviceMonitorLogic
    {
        DeviceType DeviceType { get; }

        Task Init();

        Task<string> GetChartDataAsync();

        Task<NotificationModel> GetNotificationsAsync();

        Task<Dictionary<string, string>> GetStatisticsAsync(DateTime start, DateTime end);

    }

    public enum DeviceType
    {
        Laptop = 1,
        NetworkDevice = 2
    }
}