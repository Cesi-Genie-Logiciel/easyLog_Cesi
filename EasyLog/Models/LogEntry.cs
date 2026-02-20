namespace ProSoft.EasyLog.Models
{
    // Represent a log entry in the logging system
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string JobName { get; set; } = string.Empty;
        public string SourcePath { get; set; } = string.Empty;
        public string DestPath { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public long TransferTimeMs { get; set; }
        public long? EncryptionTimeMs { get; set; }
        public JobEventType? EventType { get; set; }
        public string? Reason { get; set; }
        public string? ContextInfo { get; set; }

        public LogEntry()
        {
            Timestamp = DateTime.Now;
        }
    }
}