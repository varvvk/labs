using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class ParallelComputingTasks
{
    static void Main(string[] args)
    {


        Task1_TPLPrimeSearch();
        Task2_CancellationTokenDemo();
        Task3_TasksWithResults();
        Task4_ContinuationTasks();
        Task5_ParallelLoops();
        Task6_ParallelInvoke();
        Task7_BlockingCollectionDemo();
        Task8_AsyncAwaitDemo().Wait();
    }

    static void Task1_TPLPrimeSearch()
    {
        Console.WriteLine("\n1: Поиск простых чисел с TPL");

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Task<List<int>> primeTask = Task.Run(() => FindPrimeNumbers(30));

        Console.WriteLine($"ID задачи: {primeTask.Id}");
        Console.WriteLine($"Статус задачи до завершения: {primeTask.Status}");

        primeTask.Wait();

        stopwatch.Stop();
        Console.WriteLine($"Статус задачи после завершения: {primeTask.Status}");
        Console.WriteLine($"Количество простых чисел: {primeTask.Result.Count}");
        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
    }

    static List<int> FindPrimeNumbers(int n)
    {
        bool[] isPrime = new bool[n + 1];
        for (int i = 0; i <= n; i++)
            isPrime[i] = true;
        isPrime[0] = isPrime[1] = false;

        for (int a = 2; a * a <= n; a++)
        {
            if (isPrime[a])
            {
                for (int i = a * a; i <= n; i += a)
                    isPrime[i] = false;
            }
        }

        List<int> primes = new List<int>();
        for (int i = 2; i <= n; i++)
        {
            if (isPrime[i]) primes.Add(i);
        }
        return primes;
    }

    static void Task2_CancellationTokenDemo()
    {
        Console.WriteLine("\n2:CancellationToken");

        CancellationTokenSource tokenSource = new CancellationTokenSource();

        CancellationToken token = tokenSource.Token;



        Task task = new Task(() =>
        {
            for (int i = 0; i < 100000; i++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Задача отменена!");
                    token.ThrowIfCancellationRequested();
                }

            }
        }, token);

        tokenSource.CancelAfter(500);

        try
        {
            task.Wait();
        }
        catch (AggregateException)
        {
            Console.WriteLine("Задача была прервана досрочно");
        }
    }

    static void Task3_TasksWithResults()
    {
        Console.WriteLine("\n3: Задачи с возвратом результата");

        Task<int> task = Task.Run(() => CalculateFirst(1));
        Task<int> task2 = Task.Run(() => CalculateSecond(10));
        Task<int> task3 = Task.Run(() => CalculateThird(20));

        Task<int> final = Task.WhenAll(task, task2, task3)
            .ContinueWith(t => t.Result.Sum());

        Console.WriteLine($"Итоговый результат: {final.Result}");
    }

    static int CalculateFirst(int x) => x + 1;
    static int CalculateSecond(int x) => x + 2;
    static int CalculateThird(int x) => x + 3;

    static void Task4_ContinuationTasks()
    {
        Console.WriteLine("\n4: Continuation Tasks");

        Task<int> newTask = Task.Run(() => 15);
        Task continTask = newTask.ContinueWith(t =>
            Console.WriteLine($"Результат предыдущей задачи: {t.Result}")
        );

        Task<int> newTask2 = Task.Run(() => 30);
        int result = newTask2.GetAwaiter().GetResult();
        Console.WriteLine($"Результат задачи (GetAwaiter): {result}");
    }

    static void Task5_ParallelLoops()
    {
        Console.WriteLine("\n5: Параллельные циклы");

        Stopwatch sequentialWatch = Stopwatch.StartNew();
        for (int i = 0; i < 1000000; i++)
        {
            Math.Sin(i);
        }
        sequentialWatch.Stop();

        Stopwatch parallelWatch = Stopwatch.StartNew();
        Parallel.For(0, 1000000, i =>
        {
            Math.Sin(i);
        });
        parallelWatch.Stop();

        Console.WriteLine($"Последовательный цикл: {sequentialWatch.ElapsedMilliseconds} мс");
        Console.WriteLine($"Параллельный цикл: {parallelWatch.ElapsedMilliseconds} мс");

        List<int> numbers = Enumerable.Range(1, 1000000).ToList();

        Stopwatch sequentialWatch2 = Stopwatch.StartNew();
        foreach (var num in numbers)
        {
            Math.Sin(num);
        }
        sequentialWatch.Stop();

        Stopwatch parallelWatch2 = Stopwatch.StartNew();
        Parallel.ForEach(numbers, num =>
        {
            Math.Sin(num);
        });
        parallelWatch.Stop();

        Console.WriteLine($"Последовательный ForEach: {sequentialWatch.ElapsedMilliseconds} мс");
        Console.WriteLine($"Параллельный ForEach: {parallelWatch.ElapsedMilliseconds} мс");
    }

    static void Task6_ParallelInvoke()
    {
        Console.WriteLine("\n6: Parallel.Invoke()");


        Parallel.Invoke(
            () => CalculateSquares(),
            () => CalculateSquareRoots(),
            () => CalculateSines()
        );
    }


    static void CalculateSquares()
    {
        Console.WriteLine("Вычисление квадратов чисел...");
        int[] numbers = Enumerable.Range(1, 10).ToArray();

        foreach (var num in numbers)
        {
            Console.WriteLine($"Квадрат числа {num}: {Math.Pow(num, 2)}");
        }
    }

    static void CalculateSquareRoots()
    {
        Console.WriteLine("Вычисление квадратных корней...");
        int[] numbers = Enumerable.Range(1, 10).ToArray();

        foreach (var num in numbers)
        {
            Console.WriteLine($"Квадратный корень числа {num}: {Math.Sqrt(num)}");
        }
    }

    static void CalculateSines()
    {
        Console.WriteLine("Вычисление синусов...");
        int[] numbers = Enumerable.Range(1, 10).ToArray();

        foreach (var num in numbers)
        {
            Console.WriteLine($"Синус числа {num}: {Math.Sin(num)}");
        }
    }



    static void Task7_BlockingCollectionDemo()
    {
        Console.WriteLine("\n7: BlockingCollection");

        BlockingCollection<string> furniture = new BlockingCollection<string>(5);

        var sellerTasks = new Task[5];
        var buyerTasks = new Task[10];

        for (int i = 0; i < 5; i++)
        {
            int sellerId = i;
            sellerTasks[i] = Task.Run(() =>
            {
                string product = $"Товар от поставщика {sellerId}";
                furniture.Add(product);
                Console.WriteLine($"Добавлен {product}");
            });
        }

        for (int i = 0; i < 10; i++)
        {
            int buyerId = i;
            buyerTasks[i] = Task.Run(() =>
            {
                try
                {
                    string product = furniture.Take();
                    Console.WriteLine($"Покупатель {buyerId} купил {product}");
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine($"Покупатель {buyerId} ничего не получил и ушел");
                }
            });
        }

        Task.WaitAll(sellerTasks);
        furniture.CompleteAdding();
        Task.WaitAll(buyerTasks);
    }

    static async Task Task8_AsyncAwaitDemo()
    {
        Console.WriteLine("\n8: Async/Await");


        string result = await PerformAsyncOperation();
        Console.WriteLine($"Результат операции: {result}");
    }

    static async Task<string> PerformAsyncOperation()
    {

        await Task.Delay(2000);
        return "Операция завершена!";
    }

}




