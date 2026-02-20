using System.Text.Json;
using ProSoft.EasyLog.Interfaces;
using ProSoft.EasyLog.Models;

namespace ProSoft.EasyLog.Implementation
{
    public class JsonLogFormatter : ILogFormatter
    {
        public string FileExtension => ".json";

        public string FormatLogEntry(LogEntry entry)
        {
            return JsonSerializer.Serialize(entry, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}