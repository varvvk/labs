using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;


namespace LR12
{
    public class SEEFileManager
    {

        public static void InspectDrive(string drivePath, string inspectDirectory)
        {
            string dirInfoPath = Path.Combine(inspectDirectory, "kvvdirinfo.txt");


            Directory.CreateDirectory(inspectDirectory);

            using (StreamWriter writer = new StreamWriter(dirInfoPath))
            {

                var directories = Directory.GetDirectories(drivePath);
                writer.WriteLine("\nДиректории:");
                foreach (var directory in directories)
                {
                    writer.WriteLine(directory);
                }


                var files = Directory.GetFiles(drivePath);
                writer.WriteLine("\nФайлы:");
                foreach (var file in files)
                {
                    writer.WriteLine(file);
                }
            }


            string copiedFilePath = Path.Combine(inspectDirectory, "kvvdirinfo_copy.txt");
            File.Copy(dirInfoPath, copiedFilePath);
            File.Delete(dirInfoPath);
        }


        public static void CopyFilesWithExtension(string sourceDirectory, string destinationDirectory, string extension)
        {
            Directory.CreateDirectory(destinationDirectory);

            var files = Directory.GetFiles(sourceDirectory, $"*{extension}");
            foreach (var file in files)
            {
                string fileName = Path.GetFileName(file);
                string destFilePath = Path.Combine(destinationDirectory, fileName);
                File.Copy(file, destFilePath, true);
            }
        }

        public static void CreateArchive(string sourceDirectory, string archivePath)
        {
            try
            {
                using (FileStream fs = File.Create(archivePath))
                using (var zipStream = new ZipOutputStream(fs))
                {
                    zipStream.SetLevel(9); // Установить уровень сжатия (0-9)
                    var files = Directory.GetFiles(sourceDirectory, "*", SearchOption.AllDirectories);

                    foreach (var file in files)
                    {
                        string relativePath = file.Substring(sourceDirectory.Length + 1);
                        var entry = new ZipEntry(relativePath)
                        {
                            DateTime = File.GetLastWriteTime(file),
                            Size = new FileInfo(file).Length
                        };

                        zipStream.PutNextEntry(entry);

                        using (FileStream inputStream = File.OpenRead(file))
                        {
                            inputStream.CopyTo(zipStream);
                        }

                        zipStream.CloseEntry();
                    }
                }

                Console.WriteLine($"Архивирование завершено: {archivePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании архива: {ex.Message}");
            }
        }



        public static void ExtractArchive(string archivePath, string destinationDirectory)
{
    try
    {
        using (FileStream fs = File.OpenRead(archivePath))
        using (var zipStream = new ZipInputStream(fs))
        {
            Directory.CreateDirectory(destinationDirectory);

            ZipEntry entry;
            while ((entry = zipStream.GetNextEntry()) != null)
            {
                string outPath = Path.Combine(destinationDirectory, entry.Name);

                if (entry.IsDirectory)
                {
                    Directory.CreateDirectory(outPath);
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(outPath));
                    using (FileStream outputStream = File.Create(outPath))
                    {
                        zipStream.CopyTo(outputStream);
                    }
                }
            }
        }

        Console.WriteLine($"Разархивирование завершено в директорию: {destinationDirectory}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при разархивировании: {ex.Message}");
    }
}




        public static void Execute(string drivePath, string inspectDirectory, string filesDirectory, string extension, string extractedDirectory)
        {

            InspectDrive(drivePath, inspectDirectory);


            string tempFilesDirectory = Path.Combine(inspectDirectory, "KVVFiles");
            CopyFilesWithExtension(filesDirectory, tempFilesDirectory, extension);


            Directory.Move(tempFilesDirectory, Path.Combine(inspectDirectory, "KVVFiles"));


            string archivePath = Path.Combine(inspectDirectory, "KVVFiles.zip");
            CreateArchive(Path.Combine(inspectDirectory, "KVVFiles"), archivePath);
            ExtractArchive(archivePath, extractedDirectory);
        }

    }
}
