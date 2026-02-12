using ProSoft.EasyLog.Models;

namespace ProSoft.EasyLog.Interfaces
{

    /// Interface for log entry formatting strategies
    /// Implements Strategy pattern for format selection (JSON, XML, etc.)
    /// Separates formatting logic from file I/O operations
    /// Corresponds to ILogFormatter interface in UML diagram v1.1

    public interface ILogFormatter
    {
        /// Formats a log entry into a string representation
        /// Does NOT write to file - only converts LogEntry to formatted string
        /// This separation allows the same formatter to be used for files, databases, etc.

        /// <param name="entry">Log entry containing backup operation details</param>
        /// <returns>Formatted string (JSON, XML, or other format depending on implementation)</returns>
        string FormatLogEntry(LogEntry entry);

        /// Gets the file extension for this format
        /// Used by Logger to generate appropriate log filenames (e.g., ".json", ".xml")

        string FileExtension { get; }
    }
}
