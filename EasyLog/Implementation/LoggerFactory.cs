using ProSoft.EasyLog.Interfaces;

namespace ProSoft.EasyLog.Implementation
{
    /// Factory for creating Logger instances with appropriate formatter
    /// Implements Factory design pattern for centralized object creation
    /// Not explicitly shown in UML diagram v1.1 but useful for implementation
    /// Simplifies Logger instantiation with correct ILogFormatter based on settings

    public static class LoggerFactory
    {

        /// Creates a Logger instance with the specified format
        /// Instantiates the correct ILogFormatter implementation (JSON or XML)
        /// Then injects it into a Logger instance
    
        /// <param name="format">Desired log format from settings (JSON or XML)</param>
        /// <param name="logDirectory">Directory where log files will be stored</param>
        /// <returns>Configured Logger instance with appropriate formatter</returns>
        /// <exception cref="ArgumentException">Thrown when an unsupported format is requested</exception>
        public static ILogger CreateLogger(LogFormat format, string logDirectory)
        {
            // Create the appropriate formatter based on format parameter
            ILogFormatter formatter = format switch
            {
                LogFormat.JSON => new JsonLogFormatter(),
                LogFormat.XML => new XmlLogFormatter(),
                _ => throw new ArgumentException($"Unsupported log format: {format}", nameof(format))
            };

            // Inject formatter into Logger and return
            return new Logger(formatter, logDirectory);
        }
    }
}