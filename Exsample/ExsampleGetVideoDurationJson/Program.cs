using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;

namespace Exsample.GetVideoDuration.Json
{
    class Program
    {
        [DataContract]
        public class Person
        {
            [DataMember]
            public string duration { get; set; }
        }

        private static string Serialize(object graph)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(graph.GetType());
                serializer.WriteObject(stream, graph);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        private static T Deserialize<T>(string message)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(message)))
            {
                //var setting = new DataContractJsonSerializerSettings()
                //{
                //    UseSimpleDictionaryFormat = true,
                //};
//                var serializer = new DataContractJsonSerializer(typeof(T), /*setting*/);
                var serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(stream);
            }
        }

        static void Main(string[] args)
        {
            string currentPath = Directory.GetCurrentDirectory();

            Console.Write("Video File Duretion(mp4) = {0}\r\n\r\n", getVideoDurationJson(currentPath + @"\Test.mp4", true));
            Console.Write("Video File Duretion(flv) = {0}\r\n\r\n", getVideoDurationJson(currentPath + @"\Test.flv", true));

            Console.Write("press any key...\r\n");
            Console.ReadKey();
        }

        private static string getVideoDurationJson(string path, bool decimalpoint)
        {
            try
            {
                string currentPath = Directory.GetCurrentDirectory();
//                string commnad = string.Format(" -v error -show_format -show_entries format=duration -sexagesimal -print_format json \"{0}\"", path);
                string commnad = string.Format(" -v error -show_format -show_entries format=duration -sexagesimal -of csv \"{0}\"", path);

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

                    // リストをデシリアライズ
//                    string json = Serialize(duration);
                    var pDeserializeList = Deserialize<IList<Person>>(duration.Replace("\r\n",""));

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
    }
}
