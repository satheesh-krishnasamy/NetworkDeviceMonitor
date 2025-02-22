namespace WorkStationAssistant.Model
{
    internal sealed class LastMessageTracking
    {
        public string lastMessageShown = string.Empty;
        public DateTime nextRefreshAt = DateTime.MinValue;
    }
}