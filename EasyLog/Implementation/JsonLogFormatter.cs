using System.Text.Json;
using ProSoft.EasyLog.Interfaces;
using ProSoft.EasyLog.Models;

namespace ProSoft.EasyLog.Implementation
{
    /// JSON implementation of ILogFormatter
    /// Converts log entries to JSON format using System.Text.Json
    /// Produces human-readable indented JSON output
    /// Corresponds to JsonLogFormatter in UML diagram v1.1
    public class JsonLogFormatter : ILogFormatter
    {
        /// Gets the file extension for JSON logs
        /// Returns ".json" for proper daily log file naming
        public string FileExtension => ".json";

        /// Formats a log entry into JSON string format
        /// Uses indented formatting for human readability
        /// Does not write to file - only returns formatted string

        /// <param name="entry">Log entry to format</param>
        /// <returns>JSON string representation of the log entry</returns>
        public string FormatLogEntry(LogEntry entry)
        {
            // Configure JSON serialization options
            var options = new JsonSerializerOptions
            {
                WriteIndented = true  // Pretty print for readability
            };

            // Serialize LogEntry object to JSON string
            return JsonSerializer.Serialize(entry, options);
        }
    }
}