using ProSoft.EasyLog;
using ProSoft.EasyLog.Implementation;

namespace EasyLog.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Testing EasyLog v1.1...");

            // Test JSON format
            var jsonLogger = LoggerFactory.CreateLogger(LogFormat.JSON, @"C:\Temp\Logs");
            jsonLogger.LogFileTransfer("TestBackup_JSON",
                                      @"C:\source\file.txt",
                                      @"D:\dest\file.txt",
                                      1024000,
                                      523);
            Console.WriteLine("JSON log created successfully!");

            // Test XML format
            var xmlLogger = LoggerFactory.CreateLogger(LogFormat.XML, @"C:\Temp\Logs");
            xmlLogger.LogFileTransfer("TestBackup_XML",
                                     @"C:\source\file.txt",
                                     @"D:\dest\file.txt",
                                     2048000,
                                     745);
            Console.WriteLine("XML log created successfully!");

            Console.WriteLine("\nTests completed! Check logs at C:\\Temp\\Logs");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}