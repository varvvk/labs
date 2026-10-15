using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR12
{
    public class KVVFileInfo
    {
        public static void GetFileInfo(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);

                if (!fileInfo.Exists)
                {
                    Console.WriteLine("Файл не найден.");
                    return;
                }

                Console.WriteLine($"Полный путь: {fileInfo.FullName}");
                Console.WriteLine($"Размер: {fileInfo.Length / 1024.0:F2} КБ");
                Console.WriteLine($"Расширение: {fileInfo.Extension}");
                Console.WriteLine($"Имя файла: {fileInfo.Name}");
                Console.WriteLine($"Дата создания: {fileInfo.CreationTime}");
                Console.WriteLine($"Дата последнего изменения: {fileInfo.LastWriteTime}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения информации о файле: {ex.Message}");
            }
        }
    }
}
