using NetworkDeviceMonitor.Lib.Model;
using System.Threading.Tasks;

namespace NetworkDeviceMonitor.Lib.Logic
{
    internal interface IJioDeviceMonitorLogic
    {
        Task<StDevResponseRoot> GetDeviceDetailsAsync();

        OnNotifications NotificationEvent { get; set; }
    }
}