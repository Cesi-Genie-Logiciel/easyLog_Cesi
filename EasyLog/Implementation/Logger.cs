using ProSoft.EasyLog.Interfaces;
using ProSoft.EasyLog.Models;
using ProSoft.EasyLog.Utilities;

namespace ProSoft.EasyLog.Implementation
{
    public class Logger : ILogger
    {
        private readonly ILogFormatter _formatter;
        private readonly string _logDirectory;
        private readonly object _lock = new object();

        public Logger(ILogFormatter formatter, string logDirectory)
        {
            _formatter = formatter;
            _logDirectory = logDirectory;

            if (!Directory.Exists(_logDirectory))
                Directory.CreateDirectory(_logDirectory);
        }

        public void LogFileTransfer(string jobName, string sourceFile, string destFile,
                                    long fileSize, long durationMs)
        {
            WriteEntry(new LogEntry
            {
                JobName = jobName,
                SourcePath = PathConverter.ToUncPath(sourceFile),
                DestPath = PathConverter.ToUncPath(destFile),
                FileSize = fileSize,
                TransferTimeMs = durationMs
            });
        }

        public void LogFileTransfer(string jobName, string sourceFile, string destFile,
                                    long fileSize, long durationMs, long encryptionTimeMs)
        {
            WriteEntry(new LogEntry
            {
                JobName = jobName,
                SourcePath = PathConverter.ToUncPath(sourceFile),
                DestPath = PathConverter.ToUncPath(destFile),
                FileSize = fileSize,
                TransferTimeMs = durationMs,
                EncryptionTimeMs = encryptionTimeMs
            });
        }

        public void LogJobEvent(string jobName, JobEventType eventType,
                               string? reason = null, string? contextInfo = null)
        {
            WriteEntry(new LogEntry
            {
                JobName = jobName,
                EventType = eventType,
                Reason = reason,
                ContextInfo = contextInfo
            });
        }

        public void UpdateStateToDisk()
        {
            // to be implemented in future versions if we decide to buffer log entries in memory
        }

        private void WriteEntry(LogEntry entry)
        {
            lock (_lock)
            {
                string formatted = _formatter.FormatLogEntry(entry);
                string fileName = $"log_{DateTime.Now:yyyy-MM-dd}{_formatter.FileExtension}";
                string filePath = Path.Combine(_logDirectory, fileName);
                AppendToFile(filePath, formatted);
            }
        }

        private void AppendToFile(string filePath, string content)
        {
            if (!File.Exists(filePath))
            {
                if (_formatter.FileExtension == ".json")
                    File.WriteAllText(filePath, $"[{Environment.NewLine}{content}{Environment.NewLine}]");
                else if (_formatter.FileExtension == ".xml")
                    File.WriteAllText(filePath, $"<LogEntries>{Environment.NewLine}{content}{Environment.NewLine}</LogEntries>");
                else
                    File.WriteAllText(filePath, content + Environment.NewLine);
            }
            else
            {
                string existing = File.ReadAllText(filePath).TrimEnd();

                if (_formatter.FileExtension == ".json" && existing.EndsWith("]"))
                {
                    existing = existing.Substring(0, existing.Length - 1);
                    existing += $",{Environment.NewLine}{content}{Environment.NewLine}]";
                    File.WriteAllText(filePath, existing);
                }
                else if (_formatter.FileExtension == ".xml")
                {
                    existing = existing.Replace("</LogEntries>", $"{content}{Environment.NewLine}</LogEntries>");
                    File.WriteAllText(filePath, existing);
                }
                else
                {
                    File.AppendAllText(filePath, content + Environment.NewLine);
                }
            }
        }
    }
}