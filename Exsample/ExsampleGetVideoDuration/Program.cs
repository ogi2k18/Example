using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentPath = Directory.GetCurrentDirectory();

            Console.Write("Test:Decimal point valid\r\n");
            Console.Write(" PRINT_FORMAT=DEFAULT Test\r\n");
            Console.Write("  Video File Duretion(mp4) = {0}\r\n", getVideoDuration(currentPath + @"\Test.mp4", true));
            Console.Write("  Video File Duretion(flv) = {0}\r\n", getVideoDuration(currentPath + @"\Test.flv", true));
            Console.Write(" PRINT_FORMAT=CSV\r\n");
            Console.Write("  Video File Duretion(mp4) = {0}\r\n", getVideoDurationCSV(currentPath + @"\Test.mp4", true));
            Console.Write("  Video File Duretion(flv) = {0}\r\n\r\n", getVideoDurationCSV(currentPath + @"\Test.flv", true));

            Console.Write("Test:Decimal point invalid\r\n");
            Console.Write(" PRINT_FORMAT=DEFAULT Test\r\n");
            Console.Write("  Video File Duretion(mp4) = {0}\r\n", getVideoDuration(currentPath + @"\Test.mp4", false));
            Console.Write("  Video File Duretion(flv) = {0}\r\n\r\n", getVideoDuration(currentPath + @"\Test.flv", false));
            Console.Write(" PRINT_FORMAT=CSV\r\n");
            Console.Write("  Video File Duretion(mp4) = {0}\r\n", getVideoDurationCSV(currentPath + @"\Test.mp4", false));
            Console.Write("  Video File Duretion(flv) = {0}\r\n\r\n", getVideoDurationCSV(currentPath + @"\Test.flv", false));

            Console.Write("Class usage example\r\n");
            Console.Write("  Video File Duretion(flv) = {0}\r\n", Example.FFmpeg.getVideoDuration(currentPath + @"\Test.flv", false));
            Console.Write("  Video File Duretion(flv) = {0}\r\n\r\n", Example.FFmpeg.getVideoDuration(currentPath + @"\Test.flv"));

            Console.Write("press any key...\r\n");
            Console.ReadKey();
        }

        private static string getVideoDuration(string path, bool decimalpoint)
        {
            try
            {
                string currentPath = Directory.GetCurrentDirectory();
                string commnad = string.Format("-v error -show_format -show_entries format=duration -sexagesimal -of default=noprint_wrappers=1:nokey=1 \"{0}\"", path);

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

                    string duration = process.StandardOutput.ReadToEnd();

                    if (decimalpoint)
                        duration = duration.Substring(0, duration.IndexOf("\r"));
                    else
                        duration = duration.Substring(0, duration.IndexOf("."));

                    return duration;
                }
            }
            catch
            {
                return null;
            }
        }
        private static string getVideoDurationCSV(string path, bool decimalpoint)
        {
            try
            {
                string currentPath = Directory.GetCurrentDirectory();
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
