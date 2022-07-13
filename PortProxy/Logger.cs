using System;
using System.IO;
using System.Web;

namespace PortProxy
{

    public static class Logger
    {
        public static Logging Logging { get; set; } = new Logging();

        public static void Log(string Text)
        {
            Logging.WriteLogToFile($"{DateTime.Now}[][]{Text}");
        }



        public static void Error(string Message)
        {
            Logging.WriteErrorToFile($"{DateTime.Now}[][]{Message}");
        }

        public static void Error(Exception Ex)
        {
            Logging.WriteErrorToFile(
                $"{DateTime.Now}[]\r\n" +
                $"Message: {Ex.Message}\r\n" +
                $"StackTrace: {Ex.StackTrace}\r\n"
            );
        }

    }


    public class Logging
    {
        public Logging() { }

        private static readonly object locker = new object();



        public void WriteLogToFile(string message)
        {
            lock (locker)
            {
                var dirLog = Environment.CurrentDirectory + "\\Logs";
                Directory.CreateDirectory(dirLog);
                StreamWriter SW;
                SW = File.AppendText($"{dirLog}\\Logs {DateTime.Now.ToString("yyyyMM")}.txt");
                SW.WriteLine(message);
                SW.Close();
                if (SW != null)
                    SW.Dispose();
            }
        }


        public void WriteErrorToFile(string message)
        {
            lock (locker)
            {
                var dirLog = Environment.CurrentDirectory + "\\Logs";
                Directory.CreateDirectory(dirLog);


                StreamWriter SW;
                SW = File.AppendText($"{dirLog}\\ErrorLogs {DateTime.Now.ToString("yyyyMM")}.txt");
                SW.WriteLine(message);
                SW.Close();
                if (SW != null)
                    SW.Dispose();
            }
        }
    }
}
