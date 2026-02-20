using ProSoft.EasyLog.Models;

namespace ProSoft.EasyLog.Interfaces
{
    // Logger interface defining methods for logging file transfers and job events. 
    public interface ILogger
    {
        void LogFileTransfer(string jobName, string sourceFile, string destFile,
                            long fileSize, long durationMs);

        void LogFileTransfer(string jobName, string sourceFile, string destFile,
                            long fileSize, long durationMs, long encryptionTimeMs);

        void LogJobEvent(string jobName, JobEventType eventType,
                        string? reason = null, string? contextInfo = null);

        void UpdateStateToDisk();
    }
}