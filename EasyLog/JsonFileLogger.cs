using System.Text.Json;
using ProSoft.EasyLog.Models;
using ProSoft.EasyLog.Utilities;

namespace ProSoft.EasyLog
{
    /// <summary>
    /// Simple JSON file logger for writing log entries
    /// Thread-safe and synchronous implementation
    /// </summary>
    public class JsonFileLogger
    {
        private readonly string _logDirectory;
        private readonly object _lockObject;

        /// <summary>
        /// Creates a new JsonFileLogger
        /// </summary>
        /// <param name="logDirectory">Directory where log files will be stored</param>
        public JsonFileLogger(string logDirectory)
        {
            _logDirectory = logDirectory;
            _lockObject = new object();

            // Create log directory if it doesn't exist
            Directory.CreateDirectory(_logDirectory);
        }

        /// <summary>
        /// Writes a log entry to the daily log file
        /// </summary>
        public void WriteLog(LogEntry entry)
        {
            // Convert paths to UNC format
            entry.SourceFilePath = PathConverter.ToUncPath(entry.SourceFilePath);
            entry.TargetFilePath = PathConverter.ToUncPath(entry.TargetFilePath);

            // Thread-safe file writing
            lock (_lockObject)
            {
                string fileName = $"log_{DateTime.Now:yyyy-MM-dd}.json";
                string filePath = Path.Combine(_logDirectory, fileName);

                // Read existing entries
                List<LogEntry> entries = new List<LogEntry>();
                if (File.Exists(filePath))
                {
                    try
                    {
                        string existingContent = File.ReadAllText(filePath);
                        if (!string.IsNullOrWhiteSpace(existingContent))
                        {
                            entries = JsonSerializer.Deserialize<List<LogEntry>>(existingContent)
                                ?? new List<LogEntry>();
                        }
                    }
                    catch (JsonException)
                    {
                        // If file is corrupted, start fresh
                        entries = new List<LogEntry>();
                    }
                }

                // Add new entry
                entries.Add(entry);

                // Write all entries back with indentation
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                string jsonContent = JsonSerializer.Serialize(entries, options);
                File.WriteAllText(filePath, jsonContent);
            }
        }

        /// <summary>
        /// Writes a log entry with all parameters
        /// Helper method for convenience
        /// </summary>
        public void WriteLog(
            string backupName,
            string sourceFilePath,
            string targetFilePath,
            long fileSize,
            long transferTime)
        {
            var entry = new LogEntry
            {
                BackupName = backupName,
                SourceFilePath = sourceFilePath,
                TargetFilePath = targetFilePath,
                FileSize = fileSize,
                TransferTime = transferTime
            };

            WriteLog(entry);
        }
    }
}
