namespace windows_watchdog_cs.Models
{
    public class MonitoredProcess
    {
        public string ProcessName { get; set; }
        public string FileName { get; set; }
        public bool IsMonitored { get; set; }
    }
}
