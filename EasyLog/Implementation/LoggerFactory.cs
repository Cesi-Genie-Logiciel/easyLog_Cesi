using ProSoft.EasyLog.Interfaces;

namespace ProSoft.EasyLog.Implementation
{
    // Factory created to centralize the creation of loggers and ensure that the correct formatter is used based on the specified format.
    public static class LoggerFactory
    {
        public static ILogger Create(LogFormat format, string logDirectory)
        {
            ILogFormatter formatter = format switch
            {
                LogFormat.JSON => new JsonLogFormatter(),
                LogFormat.XML => new XmlLogFormatter(),
                _ => throw new ArgumentException($"Format non supporte : {format}")
            };

            return new Logger(formatter, logDirectory);
        }

        // redundant method for backward compatibility with version 1.0, can be removed in future versions
        public static ILogger CreateLogger(LogFormat format, string logDirectory)
        {
            return Create(format, logDirectory);
        }
    }
}