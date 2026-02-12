namespace ProSoft.EasyLog.Interfaces
{
    /// Main logger interface for writing backup operation logs
    /// Defines the contract for all logger implementations
    /// Corresponds to ILogger interface in UML diagram v1.1
    public interface ILogger
    {
        /// Logs a file transfer operation during backup
        /// Records source file, destination file, size, and duration
  
        /// <param name="backupName">Name identifier of the backup job</param>
        /// <param name="sourceFile">Original file path (local or network)</param>
        /// <param name="destFile">Destination file path (local or network)</param>
        /// <param name="fileSize">Size of the transferred file in bytes</param>
        /// <param name="durationMs">Time taken for transfer operation in milliseconds</param>
        void LogFileTransfer(string backupName, string sourceFile, string destFile,
                            long fileSize, long durationMs);

        /// Updates the current backup state to persistent storage
        /// Used for tracking backup progress and status
        /// Will be implemented in future versions
        void UpdateStateToDisk();
    }
}
