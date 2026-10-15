using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR12
{
    public class KVVLog
    {
        private string logFile;


        public KVVLog(string logFile = "kvvlogfile.txt")
        {
            this.logFile = logFile;
        }


        public void WriteLog(string action, string fileName = "", string filePath = "")
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFile, true))
                {
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string logEntry = $"{timestamp} - Action: {action}, File Name: {fileName}, File Path: {filePath}";
                    writer.WriteLine(logEntry);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"Ошибка записи в файл: {e.Message}");
            }
        }


        public string[] ReadLog()
        {
            try
            {
                return File.ReadAllLines(logFile);
            }
            catch (IOException e)
            {
                Console.WriteLine($"Ошибка чтения файла: {e.Message}");
                return Array.Empty<string>();
            }
        }

        public string[] SearchLog(string searchTerm)
        {
            try
            {
                var lines = File.ReadAllLines(logFile);
                return Array.FindAll(lines, line => line.Contains(searchTerm));
            }
            catch (IOException e)
            {
                Console.WriteLine($"Ошибка чтения файла: {e.Message}");
                return Array.Empty<string>();
            }
        }
    }
}
