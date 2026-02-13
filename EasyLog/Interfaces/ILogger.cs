using ProSoft.EasyLog.Models;

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

        

        /// <summary>
        /// Logs a file transfer operation during backup (v2.0+).
        /// Records source file, destination file, size, transfer duration and encryption duration.
        /// </summary>
        /// <param name="encryptionTimeMs">0=no encryption, >0=encryption duration (ms), <0=error code</param>
        void LogFileTransfer(string backupName, string sourceFile, string destFile,
                            long fileSize, long durationMs, long encryptionTimeMs);

        /// <summary>
        /// Logs a job-level event (not tied to a file transfer).
        /// Used for business software detection (v2.0+).
        /// </summary>
        void LogJobEvent(string backupName, JobEventType eventType, string? reason = null, string? businessSoftware = null);

/// Updates the current backup state to persistent storage
        /// Used for tracking backup progress and status
        /// Will be implemented in future versions
        void UpdateStateToDisk();
    }
}
