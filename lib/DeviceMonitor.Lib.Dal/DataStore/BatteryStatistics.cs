using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceMonitor.Lib.Dal.DataStore
{
    public sealed class BatteryStatistics
    {
        public string SessionId { get; set; }

        public DateTime StartedDateTime{ get; set; }

        public DateTime EndedDateTime { get; set; }

        public int StartPercentage { get; set; }

        public int EndPercentage { get; set; }

    }
}
