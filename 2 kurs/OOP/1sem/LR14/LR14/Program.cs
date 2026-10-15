using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Timers;

namespace LR14
{
    internal class Program
    {
        static ManualResetEvent pauseEvent = new ManualResetEvent(true);
        public static int k;
        private static readonly object lockObject = new object();
        static StreamWriter writer = new StreamWriter("4.txt", false);
        private static System.Timers.Timer timer;
        private static Stopwatch stopwatch;

        static void Main(string[] args)
        {

            //1
            var processes = Process.GetProcesses();
            using (StreamWriter writer = new StreamWriter("info1.txt"))
            {
                foreach (var process in processes)
                {
                    try
                    {
                        writer.WriteLine($"ID: {process.Id}, Имя: {process.ProcessName}");
                        writer.WriteLine($"Приоритет: {process.BasePriority}");
                        writer.WriteLine($"Время начала: {process.StartTime}");
                        writer.WriteLine($"Время исп. процессора: {process.TotalProcessorTime}");
                        writer.WriteLine($"Состояние: {process.Responding}");
                        writer.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        writer.WriteLine($"Process ID: {process.Id}, Имя: {process.ProcessName} - Ошибка: {ex.Message}");
                        writer.WriteLine();
                    }
                }
                Console.WriteLine($"Задание 1 в файле");
            }

            //2
            AppDomain currentDomain = AppDomain.CurrentDomain;
            using (StreamWriter writer = new StreamWriter("info2.txt"))
            {
                writer.WriteLine($"Имя домена: {currentDomain.FriendlyName}");
                writer.WriteLine("Загруженные сборки:");
                foreach (var assembly in currentDomain.GetAssemblies())
                {
                    writer.WriteLine($"- {assembly.FullName}");
                }
                Console.WriteLine($"Задание 2 в файле");
            }

            //AppDomain newDomain = AppDomain.CreateDomain("NewDomain");
            //Console.WriteLine($"Создан новый домен: {newDomain.FriendlyName}");
            //newDomain.Load("LR14");
            //AppDomain.Unload(newDomain);
            //Console.WriteLine("Новый домен выгружен.");

            //3
            Console.Write("Введите максимальное число символов: ");
            int n = int.Parse(Console.ReadLine());

            Thread primeThread = new Thread(() =>
            {
                using (StreamWriter writer = new StreamWriter("primes3.txt"))
                {
                    for (int i = 1; i <= n; i++)
                    {
                        if (IsPrime(i))
                        {
                            writer.WriteLine(i);
                            Console.WriteLine($"Простое число: {i}");
                            pauseEvent.Reset();
                            Console.WriteLine("Поток приостановлен.");
                            Thread.Sleep(1000);
                            pauseEvent.Set();
                            Console.WriteLine("Поток возобновлен.");
                            Console.WriteLine();
                        }
                    }
                    Console.WriteLine($"Поток: {Thread.CurrentThread.Name}");
                    Console.WriteLine($"Идентификатор: {Thread.CurrentThread.ManagedThreadId}");
                    Console.WriteLine($"Приоритет: {Thread.CurrentThread.Priority}");
                    Console.WriteLine($"Состояние: {Thread.CurrentThread.ThreadState}");
                }
            });

            primeThread.Name = "PrimeThread";
            primeThread.Priority = ThreadPriority.Normal;

            primeThread.Start();
            primeThread.Join();


            //4
            while(true)
            {
                Console.Write("Введите n для четных и нечетных чисел: ");
                if (int.TryParse(Console.ReadLine(), out k) && k > 0)
                    break;
                Console.WriteLine("Некорректный ввод. Введите положительное целое число.");
            }

            lock (lockObject)
            {
                writer.WriteLine("Вывод чётных чисел");
            }

            Console.WriteLine("Вывод чётных чисел");
            Thread evenThread = new Thread(WriteEvenNumbers);
            evenThread.Start();
            evenThread.Join();

            lock (lockObject)
            {
                writer.WriteLine("Вывод нечётных чисел");
            }


            Console.WriteLine("Вывод нечётных чисел");
            Thread oddThread = new Thread(WriteOddNumbers);
            oddThread.Start();
            oddThread.Join();

            lock (lockObject)
            {
                writer.WriteLine("Чередование");
            }
            Console.WriteLine("Чередование");

            evenThread = new Thread(AlternatingEvenNumbers);
            oddThread = new Thread(AlternatingOddNumbers);

            evenThread.Start();
            oddThread.Start();

            evenThread.Join();
            oddThread.Join();

            //5
            stopwatch = new Stopwatch();
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;


            stopwatch.Start();
            Console.WriteLine("Таймер запущен. Нажмите Enter для остановки.");


            Console.ReadLine();


            timer.Stop();
            stopwatch.Stop();

            Console.WriteLine($"Таймер остановлен. Время работы: {stopwatch.Elapsed.TotalSeconds} секунд.");
        }


        //3
        static bool IsPrime(int number)
        {
            if (number < 2) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0) return false;
            }
            return true;
        }

        //4
        static void WriteEvenNumbers()
        {
            for (int i = 2; i <= k; i += 2)
            {
                Console.WriteLine($"Чётное: {i}");
                writer.WriteLine(i);
                writer.Flush();
                Thread.Sleep(200);
            }
        }

        static void WriteOddNumbers()
        {
            for (int i = 1; i <= k; i += 2)
            {
                Console.WriteLine($"Нечётное: {i}");
                writer.WriteLine(i);
                writer.Flush();
                Thread.Sleep(300);
            }
        }

        static void AlternatingOddNumbers()
        {
            for (int i = 1; i <= k; i += 2)
            {
                lock (lockObject)
                {
                    Console.WriteLine($"Нечётное: {i}");
                    writer.WriteLine(i);
                    writer.Flush();
                    Thread.Sleep(300);
                    Monitor.Pulse(lockObject);
                    if (i + 2 <= k)
                        Monitor.Wait(lockObject);
                }
            }
        }

        static void AlternatingEvenNumbers()
        {
            for (int i = 2; i <= k; i += 2)
            {
                lock (lockObject)
                {
                    Console.WriteLine($"Чётное: {i}");
                    writer.WriteLine(i);
                    writer.Flush();
                    Thread.Sleep(200);
                    Monitor.Pulse(lockObject);
                    Monitor.Wait(lockObject);
                }
            }
        }

        //5
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine($"Прошло времени: {stopwatch.Elapsed.TotalSeconds} секунд.");
        }

    }
}
