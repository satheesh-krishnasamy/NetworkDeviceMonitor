using System;

namespace NetworkDeviceMonitor.Lib.Model.DataStore
{
    public class BatteryDataItem
    {
        public int BatteryPercentage { get; set; }

        public DateTime EventDateTime { get; set; }

        public bool IsCharging { get; set; }


    }
}
