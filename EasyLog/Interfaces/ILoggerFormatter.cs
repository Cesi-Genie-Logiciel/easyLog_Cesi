using ProSoft.EasyLog.Models;

namespace ProSoft.EasyLog.Interfaces
{
    // Interface to define the contract for log formatters.
    public interface ILogFormatter
    {
        string FormatLogEntry(LogEntry entry);
        string FileExtension { get; }
    }
}