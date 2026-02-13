using System.Text.Json;
using ProSoft.EasyLog.Models;
using ProSoft.EasyLog.Utilities;

namespace ProSoft.EasyLog.Writers
{
    /// <summary>
    /// JSON file logger implementation
    /// Writes log entries to daily JSON files
    /// Thread-safe implementation
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
                string logFileName = $"log_{DateTime.Now:yyyy-MM-dd}.json";
                string logFilePath = Path.Combine(_logDirectory, logFileName);

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

                List<LogEntry> entries = new List<LogEntry>();
                if (File.Exists(logFilePath))
                {
                    string jsonContent = File.ReadAllText(logFilePath);
                    entries = JsonSerializer.Deserialize<List<LogEntry>>(jsonContent)
                              ?? new List<LogEntry>();
                }

                entries.Add(entry);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                string jsonOutput = JsonSerializer.Serialize(entries, options);
                File.WriteAllText(logFilePath, jsonOutput);
            }
        }
    }
}