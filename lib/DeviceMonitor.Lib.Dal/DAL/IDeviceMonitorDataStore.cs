using DeviceMonitor.Lib.Dal.DataStore;
using WorkStationAssistant.Lib.Model.DataStore;

namespace WorkStationAssistant.Lib.DAL
{

    public interface IDataStoreReadOnly
    {
        Task<IEnumerable<BatteryDataItem>> GetAsync(DateTime startDateTime, DateTime endDateTime);
        Task<IEnumerable<BatteryStatistics>> GetBatteryStatisticsAsync(DateTime startDateTime, DateTime endDateTime);
        Task<IEnumerable<BatteryDataItem>> GetLastBatteryChargingSessionsAsync(DateTime startDateTime, DateTime endDateTime);

    }

    public interface IDeviceMonitorDataStore : IDataStoreReadOnly
    {
        Task<bool> SaveAsync(BatteryDataItem batteryData);

    }
}