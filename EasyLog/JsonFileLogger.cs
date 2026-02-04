using System.Text.Json;
using ProSoft.EasyLog.Models;
using ProSoft.EasyLog.Utilities;

namespace ProSoft.EasyLog
{
    /// <summary>
    /// Main logger class that writes log entries to daily JSON files
    /// Thread-safe implementation using lock mechanism
    /// </summary>
    public class JsonFileLogger
    {
        private readonly string _logDirectory;
        private readonly object _lockObject = new object();

        /// <summary>
        /// Initializes a new instance of JsonFileLogger
        /// </summary>
        /// <param name="logDirectory">Directory where log files will be stored</param>
        public JsonFileLogger(string logDirectory)
        {
            _logDirectory = logDirectory;

            // Create directory if it doesn't exist
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
        }

        /// <summary>
        /// Writes a log entry to the daily JSON file
        /// </summary>
        public void WriteLog(string backupName, string sourceFilePath, string targetFilePath,
                             long fileSize, long transferTime)
        {
            lock (_lockObject)
            {
                // Get today's log file path
                string logFileName = $"log_{DateTime.Now:yyyy-MM-dd}.json";
                string logFilePath = Path.Combine(_logDirectory, logFileName);

                // Create log entry
                var entry = new LogEntry
                {
                    Timestamp = DateTime.Now,
                    BackupName = backupName,
                    SourceFilePath = PathConverter.ToUncPath(sourceFilePath),
                    TargetFilePath = PathConverter.ToUncPath(targetFilePath),
                    FileSize = fileSize,
                    TransferTime = transferTime,
                    EncryptionTime = null
                };

                // Read existing entries
                List<LogEntry> entries = new List<LogEntry>();

                if (File.Exists(logFilePath))
                {
                    string jsonContent = File.ReadAllText(logFilePath);
                    entries = JsonSerializer.Deserialize<List<LogEntry>>(jsonContent)
                              ?? new List<LogEntry>();
                }

                // Add new entry
                entries.Add(entry);

                // Serialize with indentation
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string jsonOutput = JsonSerializer.Serialize(entries, options);

                // Write to file
                File.WriteAllText(logFilePath, jsonOutput);
            }
        }

        /// <summary>
        /// Writes a log entry using a LogEntry object
        /// </summary>
        public void WriteLog(LogEntry entry)
        {
            WriteLog(
                entry.BackupName,
                entry.SourceFilePath,
                entry.TargetFilePath,
                entry.FileSize,
                entry.TransferTime
            );
        }
    }
}
