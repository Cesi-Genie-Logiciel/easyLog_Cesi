namespace ProSoft.EasyLog.Models
{
    /// <summary>
    /// Represents a single log entry for backup operations
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Timestamp of the log entry
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Name of the backup job
        /// </summary>
        public string BackupName { get; set; } = string.Empty;

        /// <summary>
        /// Full UNC path of the source file
        /// </summary>
        public string SourceFilePath { get; set; } = string.Empty;

        /// <summary>
        /// Full UNC path of the target file
        /// </summary>
        public string TargetFilePath { get; set; } = string.Empty;

        /// <summary>
        /// Size of the file in bytes
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Transfer time in milliseconds (negative if error occurred)
        /// </summary>
        public long TransferTime { get; set; }

        /// <summary>
        /// Encryption time in milliseconds (for version 2.0+)
        /// 0 = no encryption, >0 = encryption time, <0 = error code
        /// </summary>
        public long? EncryptionTime { get; set; }

        /// <summary>
        /// Constructor initializes timestamp to current time
        /// </summary>
        public LogEntry()
        {
            Timestamp = DateTime.Now;
        }
    }
}
