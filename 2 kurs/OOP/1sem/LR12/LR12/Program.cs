using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR12
{
    public class LogFileInfo
    {
        public static void ProcessLogFile(string logFilePath, DateTime startDateTime, DateTime endDateTime, string keyword)
        {
            try
            {

                var lines = File.ReadAllLines(logFilePath);


                var filteredLines = lines.Where(line =>
                {

                    var dateTimePart = line.Substring(0, 19);
                    DateTime logDateTime;

                    if (DateTime.TryParse(dateTimePart, out logDateTime))
                    {

                        return logDateTime >= startDateTime && logDateTime <= endDateTime &&
                               line.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0
;
                    }
                    return false;
                }).ToList();


                Console.WriteLine($"Количество записей: {filteredLines.Count}");
                foreach (var line in filteredLines)
                {
                    Console.WriteLine(line);
                }


                DateTime currentHour = DateTime.Now.Date.AddHours(DateTime.Now.Hour);
                var currentHourLines = lines.Where(line =>
                {
                    var dateTimePart = line.Substring(0, 19);
                    DateTime logDateTime;
                    return DateTime.TryParse(dateTimePart, out logDateTime) && logDateTime >= currentHour;
                }).ToList();


                File.WriteAllLines(logFilePath, currentHourLines);

                Console.WriteLine("\nЗаписи за текущий час были сохранены.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            KVVLog logger = new KVVLog();


            logger.WriteLog("Создание файла", "kvvlogfile.txt", @"C:\\labs\\OOP\\LR12\\LR12\\bin\\Debug\\kvvlogfile.txt");

            Console.WriteLine("Информация о дисках:");
            KVVDiskInfo.GetDiskInfo();


            string driveLetter = "C:\\";
            Console.WriteLine($"Свободное место на диске {driveLetter}: {KVVDiskInfo.GetFreeSpace(driveLetter) / (1024 * 1024 * 1024)} ГБ");
            Console.WriteLine($"Файловая система диска {driveLetter}: {KVVDiskInfo.GetFileSystem(driveLetter)}");


            string filePath = @"C:\\labs\\OOP\\LR12\\LR12\\bin\\Debug\\kvvlogfile.txt";
            Console.WriteLine("\nИнформация о файле:");
            KVVFileInfo.GetFileInfo(filePath);

            string directoryPath = @"C:\\labs\\OOP\\LR12\\LR12\\bin\\Debug";

            SEEDirInfo.Demonstrate(directoryPath);



            string[] directories = { @"C:\Dir", @"C:\KVVInspect", @"C:\SourceFiles", @"C:\ExtractedFiles" };
            foreach (string path in directories)
            {
                CreateDirectoryIfNotExists(path);
            }


            void CreateDirectoryIfNotExists(string path)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Console.WriteLine($"Директория {path} была создана.");
                }
                else
                {
                    Console.WriteLine($"Директория {path} уже существует.");
                }
            }

            string archivePath = Path.Combine(@"C:\KVVInspect", "KVVFiles.zip");
            SEEFileManager.CreateArchive(Path.Combine(@"C:\KVVInspect", "KVVFiles"), archivePath);


            SEEFileManager.ExtractArchive(archivePath, @"C:\ExtractedFiles");



            string logFilePath = @"C:\\labs\\OOP\\LR12\\LR12\\bin\\Debug\\kvvlogfile.txt";
            DateTime startDateTime = new DateTime(2024, 12, 10, 10, 0, 0);
            DateTime endDateTime = new DateTime(2024, 12, 10, 12, 0, 0);
            string keyword = "KVV";

            LogFileInfo.ProcessLogFile(logFilePath, startDateTime, endDateTime, keyword);
        }
    }
}
