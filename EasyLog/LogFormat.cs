using ProSoft.EasyLog;
using System.Net;

namespace ProSoft.EasyLog
{
    /// Enumeration of supported log file formats
    /// Used to determine which formatter to instantiate
    /// Corresponds to LogFormat enum in UML diagram v1.1

    public enum LogFormat
    {
        /// JavaScript Object Notation format
        /// Default format for backward compatibility with version 1.0

        JSON,
        /// Extensible Markup Language format
        /// Added in version 1.1 for enhanced readability and structure

        XML
    }
}
