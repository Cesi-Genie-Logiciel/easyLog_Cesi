using System.Xml.Linq;
using ProSoft.EasyLog.Interfaces;
using ProSoft.EasyLog.Models;

namespace ProSoft.EasyLog.Implementation
{

    /// XML implementation of ILogFormatter
    /// Converts log entries to XML format using System.Xml.Linq
    /// Creates hierarchical structure with LogEntry as root element
    /// Corresponds to XmlLogFormatter in UML diagram v1.1

    public class XmlLogFormatter : ILogFormatter
    {

        /// Gets the file extension for XML logs
        /// Returns ".xml" for proper daily log file naming

        public string FileExtension => ".xml";
        /// Formats a log entry into XML string format
        /// Creates a hierarchical XML structure with proper indentation
        /// Does not write to file - only returns formatted string
        /// <param name="entry">Log entry to format</param>
        /// <returns>XML string representation of the log entry</returns>
        public string FormatLogEntry(LogEntry entry)
        {
            // Build XML element structure
            var logElement = new XElement("LogEntry",
                new XElement("Timestamp", entry.Timestamp.ToString("yyyy-MM-ddTHH:mm:ss")),  // ISO 8601 format
                new XElement("BackupName", entry.BackupName),
                new XElement("SourceFilePath", entry.SourceFilePath),
                new XElement("TargetFilePath", entry.TargetFilePath),
                new XElement("FileSize", entry.FileSize),
                new XElement("TransferTime", entry.TransferTime)
            );

            // Add encryption time only if present (version 2.0 feature, nullable field)
            if (entry.EncryptionTime.HasValue)
            {
                logElement.Add(new XElement("EncryptionTime", entry.EncryptionTime.Value));
            }

            if (entry.EventType.HasValue)
            {
                logElement.Add(new XElement("EventType", entry.EventType.Value.ToString()));
            }

            if (!string.IsNullOrWhiteSpace(entry.Reason))
            {
                logElement.Add(new XElement("Reason", entry.Reason));
            }

            if (!string.IsNullOrWhiteSpace(entry.BusinessSoftware))
            {
                logElement.Add(new XElement("BusinessSoftware", entry.BusinessSoftware));
            }

            // Convert XElement to indented string
            return logElement.ToString();
        }
    }
}