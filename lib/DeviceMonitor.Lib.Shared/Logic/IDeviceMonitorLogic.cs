using WorkStationAssistant.Lib.Model;

namespace DeviceMonitor.Lib.Shared.Logic
{
    public interface IDeviceMonitorLogic
    {
        DeviceType DeviceType { get; }

        Task Init();

        Task<string> GetChartDataAsync();

        Task<NotificationModel> GetNotificationsAsync();
    }

    public enum DeviceType
    {
        Laptop = 1,
        NetworkDevice = 2
    }
}