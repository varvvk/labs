using System;
using System.IO;

namespace LR4.Lec04LibN
{
    internal class Logger : ILogger
    {
        private static Logger _instance; 
        private StreamWriter _writer;  
        private bool _isStarted = false; 
        private int _logCounter = 1;   

        private string LogFileName = string.Format(@"{0}/LOG{1}.txt",
            Directory.GetCurrentDirectory(),
            DateTime.Now.ToString("yyyyMMdd-HH-mm-ss"));

        private Logger()
        {
            _writer = new StreamWriter(LogFileName, true);
            _writer.WriteLine($"000001-{DateTime.Now:dd.MM.yyyy HH:mm:ss}-INIT");
            _writer.Flush();
        }

        public static ILogger create()
        {
            if (_instance == null)
                _instance = new Logger();
            return _instance;
        }

        public void start(string title)
        {
            if (_isStarted)
                log($"STRT A:{title}");
            else
            {
                log($"STRT A:{title}");
                _isStarted = true;
            }
        }

        public void log(string message)
        {
            _logCounter++;
            _writer.WriteLine($"{_logCounter:000000}-{DateTime.Now:dd.MM.yyyy HH:mm:ss}-INFO {message}");
            _writer.Flush();
        }

        public void stop()
        {
            if (_isStarted)
            {
                log("STOP A");
                _isStarted = false;
            }
            else
            {
                log("STOP A");
            }
        }
    }
}
