using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Example
{
    public static class FFmpeg
    {
        public static string CurrentPath { get; set; } = Directory.GetCurrentDirectory();

        public static string getVideoDuration(string path)
        {
            return getVideoDuration(path, false);
        }
        public static string getVideoDuration(string path, bool decimalpoint)
        {
            try
            {
                string currentPath = CurrentPath;
                string commnad = string.Format("-v error -show_format -show_entries format=duration -sexagesimal -of csv=nokey=0 \"{0}\"", path);

                using (var process = new System.Diagnostics.Process())
                {
                    process.StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = currentPath + @"\ffprobe.exe",
                        Arguments = commnad,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    if (!process.Start()) return null;

                    process.WaitForExit();

                    string stream = process.StandardOutput.ReadToEnd();

                    string pattern = @"duration=(?<duration>.[^,]*)";

                    string duration = Regex.Match(stream, pattern).Groups["duration"].Value;

                    if (!decimalpoint)
                        duration = duration.Substring(0, duration.IndexOf("."));

                    return duration;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
