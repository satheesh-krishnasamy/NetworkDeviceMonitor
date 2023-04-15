using NetworkDeviceMonitor.Lib.Model.DataStore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetworkDeviceMonitor.Lib.DAL
{
    public interface INetworkDataStore
    {
        Task<IEnumerable<BatteryDataItem>> GetAsync(DateTime startDateTime, DateTime endDateTime);
        Task<bool> SaveAsync(BatteryDataItem batteryData);
    }
}