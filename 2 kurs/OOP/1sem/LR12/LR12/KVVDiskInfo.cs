using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR12
{
    public class KVVDiskInfo
    {

        public static long GetFreeSpace(string driveLetter)
        {
            DriveInfo drive = new DriveInfo(driveLetter);
            if (drive.IsReady)
            {
                return drive.AvailableFreeSpace;
            }
            throw new IOException("Диск недоступен.");
        }


        public static string GetFileSystem(string driveLetter)
        {
            DriveInfo drive = new DriveInfo(driveLetter);
            if (drive.IsReady)
            {
                return drive.DriveFormat;
            }
            throw new IOException("Диск недоступен.");
        }


        public static void GetDiskInfo()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    Console.WriteLine($"Имя: {drive.Name}");
                    Console.WriteLine($"Объем: {drive.TotalSize / (1024 * 1024 * 1024)} ГБ");
                    Console.WriteLine($"Доступный объем: {drive.AvailableFreeSpace / (1024 * 1024 * 1024)} ГБ");
                    Console.WriteLine($"Метка тома: {drive.VolumeLabel}");
                    Console.WriteLine($"Файловая система: {drive.DriveFormat}");
                    Console.WriteLine();
                }
            }
        }
    }
}
