using System.Xml.Linq;
using ProSoft.EasyLog.Interfaces;
using ProSoft.EasyLog.Models;

namespace ProSoft.EasyLog.Implementation
{
    public class XmlLogFormatter : ILogFormatter
    {
        public string FileExtension => ".xml";

        public string FormatLogEntry(LogEntry entry)
        {
            var element = new XElement("LogEntry",
                new XElement("Timestamp", entry.Timestamp.ToString("yyyy-MM-ddTHH:mm:ss")),
                new XElement("JobName", entry.JobName),
                new XElement("SourcePath", entry.SourcePath),
                new XElement("DestPath", entry.DestPath),
                new XElement("FileSize", entry.FileSize),
                new XElement("TransferTimeMs", entry.TransferTimeMs)
            );

            if (entry.EncryptionTimeMs.HasValue)
                element.Add(new XElement("EncryptionTimeMs", entry.EncryptionTimeMs.Value));

            if (entry.EventType.HasValue)
                element.Add(new XElement("EventType", entry.EventType.Value.ToString()));

            if (!string.IsNullOrWhiteSpace(entry.Reason))
                element.Add(new XElement("Reason", entry.Reason));

            if (!string.IsNullOrWhiteSpace(entry.ContextInfo))
                element.Add(new XElement("ContextInfo", entry.ContextInfo));

            return element.ToString();
        }
    }
}