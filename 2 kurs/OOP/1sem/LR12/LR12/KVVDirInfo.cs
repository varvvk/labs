using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR12
{
    public class SEEDirInfo
    {

        public static int GetFileCount(string directoryPath)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    return Directory.GetFiles(directoryPath).Length;
                }
                Console.WriteLine($"Директория {directoryPath} не существует, создаем...");
                Directory.CreateDirectory(directoryPath);
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
                return 0;
            }
        }


        public static DateTime GetCreationTime(string directoryPath)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    return Directory.GetCreationTime(directoryPath);
                }
                Console.WriteLine($"Директория {directoryPath} не существует, создаем...");
                Directory.CreateDirectory(directoryPath);
                return DateTime.Now;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
                return DateTime.MinValue;
            }
        }


        public static int GetSubdirectoryCount(string directoryPath)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    return Directory.GetDirectories(directoryPath).Length;
                }
                Console.WriteLine($"Директория {directoryPath} не существует.");
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
                return 0;
            }
        }


        public static void GetParentDirectories(string directoryPath)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    DirectoryInfo parent = Directory.GetParent(directoryPath);
                    while (parent != null)
                    {
                        Console.WriteLine($"Родительская директория: {parent.FullName}");
                        parent = parent.Parent;
                    }
                }
                else
                {
                    Console.WriteLine("Директория не существует.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка: {e.Message}");
            }
        }


        public static void Demonstrate(string directoryPath)
        {
            Console.WriteLine($"Информация о директории: {directoryPath}");
            Console.WriteLine($"Количество файлов: {GetFileCount(directoryPath)}");
            Console.WriteLine($"Время создания: {GetCreationTime(directoryPath)}");
            Console.WriteLine($"Количество поддиректориев: {GetSubdirectoryCount(directoryPath)}");
            Console.WriteLine("Список родительских директорий:");
            GetParentDirectories(directoryPath);
        }
    }
}
