using NetworkDeviceMonitor.Lib.Model;
using System.Threading.Tasks;

namespace NetworkDeviceMonitor.Lib.Logic
{
    internal interface IJioDeviceMonitorLogic
    {
        Task<StDevResponseRoot> GetDeviceDetailsAsync();
        Task<string> GetChartDataAsync();

        OnNotificationsEvent NotificationEvent { get; set; }
    }
}