using System.Text.RegularExpressions;

namespace ProSoft.EasyLog.Utilities
{
    // Convert local paths to UNC paths for better readability in logs, especially when dealing with network shares or remote machines.
    public static class PathConverter
    {
        public static string ToUncPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return path;

            if (path.StartsWith(@"\\"))
                return path;

            if (Regex.IsMatch(path, @"^[a-zA-Z]:\\"))
            {
                string drive = path.Substring(0, 1);
                string rest = path.Substring(3);
                return $@"\\{Environment.MachineName}\{drive}$\{rest}";
            }

            return path;
        }
    }
}