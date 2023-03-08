namespace NetworkDeviceMonitor.Lib.Model
{
    public enum BatteryStatus
    {
        /// <summary>
        /// Unable to determine the status of the battery at this time.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// No battery found. 
        /// Not sure when this will be reported as the jio api runs in the device.
        /// May be when the device run with direct power without battery. Untested this use case.
        /// </summary>
        NoBattery = 1,
        /// <summary>
        /// Battery is fully charged (100%).
        /// </summary>
        FullyCharged = 2,
        /// <summary>
        /// Battery is being charged. Charger is connected.
        /// </summary>
        Charging = 3,
        /// <summary>
        /// Battery is discharging. No charger is connected.
        /// </summary>
        Discharging = 4
    }
}
