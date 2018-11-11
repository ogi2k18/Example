using System;
using System.IO;

namespace Exsample.GetVideoDuration
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        private static string getVideoDuration(string path)
        {
            try
            {
                //            string currentPath = Directory.GetCurrentDirectory(); + Path.DirectorySeparatorChar;
                string currentPath = Directory.GetCurrentDirectory();
                string commnad = string.Format(" \"{0}\" -v error -show_format -show_entries format=duration -sexagesimal -of default=noprint_wrappers=1:nokey=1", path);

                using (var process = new System.Diagnostics.Process())
                {
                    process.StartInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = currentPath + @"\ffprobe.exe",
                        Arguments = commnad,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                    };
                    process.OutputDataReceived += OutputDataReceived;

                    process.Start();

                    //string duration = process.StandardOutput.ReadToEnd().Replace("\r\n", "");
                    string duration = process.StandardOutput.ReadToEnd().Replace("\r\n", "");

                    duration = duration.Substring(0, duration.IndexOf("."));

                    process.WaitForExit();

                    return duration;
                }
#if false
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = currentPath + @"\ffprobe.exe";
                process.StartInfo.Arguments = commnad;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;

                process.Start();

                string duration = process.StandardOutput.ReadToEnd().Replace("\r\n", "");

                duration = duration.Substring(0, duration.IndexOf("."));

                process.WaitForExit();
                process.Close();

                return duration;
#endif
            }
            catch
            {
                return null;
            }
        }

        static void OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
