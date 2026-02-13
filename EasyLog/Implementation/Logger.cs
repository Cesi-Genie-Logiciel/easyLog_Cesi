using ProSoft.EasyLog.Interfaces;
using ProSoft.EasyLog.Models;
using ProSoft.EasyLog.Utilities;

namespace ProSoft.EasyLog.Implementation
{
    /// Main logger implementation using Strategy pattern
    /// Uses ILogFormatter for format independence (JSON, XML, etc.)
    /// Handles file I/O operations and thread-safety
    /// Corresponds to Logger class in UML diagram v1.1

    public class Logger : ILogger
    {
        private readonly ILogFormatter _formatter;
        private readonly string _logDirectory;
        private readonly object _lockObject = new object();
        /// Initializes a new instance of Logger
        /// Implements dependency injection for ILogFormatter (Strategy pattern)
        
        /// <param name="formatter">Formatter strategy to use (JSON, XML, etc.)</param>
        /// <param name="logDirectory">Directory where log files will be stored</param>
        public Logger(ILogFormatter formatter, string logDirectory)
        {
            _formatter = formatter;
            _logDirectory = logDirectory;

            // Create log directory if it doesn't exist
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }
        /// Logs a file transfer operation
        /// Thread-safe implementation using lock mechanism to prevent concurrent write conflicts
        /// Creates daily log files with naming convention: log_YYYY-MM-DD.{ext}

        /// <param name="backupName">Name identifier of the backup job</param>
        /// <param name="sourceFile">Original file path</param>
        /// <param name="destFile">Destination file path</param>
        /// <param name="fileSize">Size of transferred file in bytes</param>
        /// <param name="durationMs">Time taken for transfer in milliseconds</param>
        public void LogFileTransfer(string backupName, string sourceFile, string destFile,
                                    long fileSize, long durationMs)
        {
            lock (_lockObject)  // Prevent race conditions when multiple backups run simultaneously
            {
                // Create log entry object
                var entry = new LogEntry
                {
                    Timestamp = DateTime.Now,
                    BackupName = backupName,
                    SourceFilePath = PathConverter.ToUncPath(sourceFile),  // Convert to UNC path
                    TargetFilePath = PathConverter.ToUncPath(destFile),    // Convert to UNC path
                    FileSize = fileSize,
                    TransferTime = durationMs,
                    EncryptionTime = null  // Not used in version 1.1
                };

                // Format entry using injected formatter (Strategy pattern in action)
                string formattedLog = _formatter.FormatLogEntry(entry);

                // Generate daily log filename with appropriate extension
                string logFileName = $"log_{DateTime.Now:yyyy-MM-dd}{_formatter.FileExtension}";
                string logFilePath = Path.Combine(_logDirectory, logFileName);

                // Write to file with format-specific handling
                AppendToLogFile(logFilePath, formattedLog);
            }
        }

        /// <summary>
        /// Logs a file transfer operation during backup (v2.0+).
        /// Records source file, destination file, size, transfer duration and encryption duration.
        /// </summary>
        /// <param name="encryptionTimeMs">0=no encryption, >0=encryption duration (ms), <0=error code</param>
        public void LogFileTransfer(string backupName, string sourceFile, string destFile,
                                    long fileSize, long durationMs, long encryptionTimeMs)
        {
            lock (_lockObject)
            {
                var entry = new LogEntry
                {
                    Timestamp = DateTime.Now,
                    BackupName = backupName,
                    SourceFilePath = PathConverter.ToUncPath(sourceFile),
                    TargetFilePath = PathConverter.ToUncPath(destFile),
                    FileSize = fileSize,
                    TransferTime = durationMs,
                    EncryptionTime = encryptionTimeMs
                };

                string formattedLog = _formatter.FormatLogEntry(entry);
                string logFileName = $"log_{DateTime.Now:yyyy-MM-dd}{_formatter.FileExtension}";
                string logFilePath = Path.Combine(_logDirectory, logFileName);
                AppendToLogFile(logFilePath, formattedLog);
            }
        }


        /// Updates backup state to disk
        /// Placeholder for state management functionality (will be implemented in future versions)
        public void UpdateStateToDisk()
        {
            // TODO: Implement state persistence for BackupState objects
            // This method is part of the ILogger interface but not used in v1.1
        }

        /// Appends formatted log to file
        /// Handles both new file creation and appending to existing files
        /// Format-specific logic for proper structure (JSON arrays, XML root elements)
        
        /// <param name="filePath">Full path to the log file</param>
        /// <param name="formattedLog">Pre-formatted log entry string</param>
        private void AppendToLogFile(string filePath, string formattedLog)
        {
            if (!File.Exists(filePath))
            {
                // New file - create with proper wrapper structure
                if (_formatter.FileExtension == ".json")
                {
                    // JSON: Create array with first entry
                    File.WriteAllText(filePath, $"[{Environment.NewLine}{formattedLog}{Environment.NewLine}]");
                }
                else if (_formatter.FileExtension == ".xml")
                {
                    // XML: Create root LogEntries element
                    File.WriteAllText(filePath, $"<LogEntries>{Environment.NewLine}{formattedLog}{Environment.NewLine}</LogEntries>");
                }
                else
                {
                    // Generic format: Just write the log
                    File.WriteAllText(filePath, formattedLog + Environment.NewLine);
                }
            }
            else
            {
                // Existing file - append entry with format-specific handling
                if (_formatter.FileExtension == ".json")
                {
                    // JSON: Insert before closing bracket to maintain valid JSON array
                    string content = File.ReadAllText(filePath);
                    content = content.TrimEnd();
                    if (content.EndsWith("]"))
                    {
                        content = content.Substring(0, content.Length - 1); // Remove closing ]
                        content += $",{Environment.NewLine}{formattedLog}{Environment.NewLine}]";
                        File.WriteAllText(filePath, content);
                    }
                }
                else if (_formatter.FileExtension == ".xml")
                {
                    // XML: Insert before closing tag to maintain valid XML structure
                    string content = File.ReadAllText(filePath);
                    content = content.Replace("</LogEntries>", $"{formattedLog}{Environment.NewLine}</LogEntries>");
                    File.WriteAllText(filePath, content);
                }
                else
                {
                    // Generic format: Simple append
                    File.AppendAllText(filePath, formattedLog + Environment.NewLine);
                }
            }
        }

        /// <summary>
        /// Logs a job event (start, completion, error, etc.) for the backup process.
        /// Includes optional reason and business software information.
        /// </summary>
        /// <param name="eventType">Type of the event (e.g., Started, Completed, Error)</param>
        /// <param name="reason">Optional reason for the event (e.g., error message)</param>
        /// <param name="businessSoftware">Optional business software context</param>
        public void LogJobEvent(string backupName, JobEventType eventType, string? reason = null, string? businessSoftware = null)
        {
            lock (_lockObject)
            {
                var entry = new LogEntry
                {
                    Timestamp = DateTime.Now,
                    BackupName = backupName,
                    SourceFilePath = string.Empty,
                    TargetFilePath = string.Empty,
                    FileSize = 0,
                    TransferTime = 0,
                    EncryptionTime = null,
                    EventType = eventType,
                    Reason = reason,
                    BusinessSoftware = businessSoftware
                };

                string formattedLog = _formatter.FormatLogEntry(entry);
                string logFileName = $"log_{DateTime.Now:yyyy-MM-dd}{_formatter.FileExtension}";
                string logFilePath = Path.Combine(_logDirectory, logFileName);
                AppendToLogFile(logFilePath, formattedLog);
            }
        }
    }
}
