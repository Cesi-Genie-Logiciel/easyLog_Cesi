using System.Text.RegularExpressions;

namespace ProSoft.EasyLog.Utilities
{
    /// <summary>
    /// Utility class to convert file paths to UNC format
    /// </summary>
    public static class PathConverter
    {
        /// <summary>
        /// Converts a local or network path to UNC format
        /// </summary>
        /// <param name="path">The path to convert</param>
        /// <returns>UNC formatted path</returns>
        public static string ToUncPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return path;

            // Already in UNC format
            if (path.StartsWith(@"\\"))
                return path;

            // Check if it's a local path (e.g., C:\folder\file.txt)
            if (Regex.IsMatch(path, @"^[a-zA-Z]:\\"))
            {
                string drive = path.Substring(0, 1);
                string remainingPath = path.Substring(3);
                string machineName = Environment.MachineName;

                return $@"\\{machineName}\{drive}$\{remainingPath}";
            }

            // If not recognized, return as-is
            return path;
        }
    }
}
