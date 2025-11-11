using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace empinquiry
{
    public static class Loggers
    {
        private static readonly object _lock = new object();

        public static void Log(string message)
        {
            try
            {
                string logDir = HttpContext.Current.Server.MapPath("~/Logs/");
                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);

                string logFile = Path.Combine(logDir, $"AppLog_{DateTime.Now:yyyyMMdd}.txt");

                string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}";

                lock (_lock)
                {
                    File.AppendAllText(logFile, logMessage + Environment.NewLine);
                }
            }
            catch
            {
                // Avoid throwing exceptions from the logger itself
            }
        }
    }
}