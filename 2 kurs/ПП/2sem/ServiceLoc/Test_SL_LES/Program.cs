using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using DAL_LES;
using SericeLocator;

namespace Test_SL_LES
{


    class Program
    {
        static void Main(string[] args)
        {
            // Создаём контейнер и регистрируем сервисы
            var serviceProvider = new ServiceCollection()
                .AddTransient<IRepository, Repository>()
                .AddScoped<ICashCelebrities>(provider =>
                new CashCelebrities(
                    provider.GetRequiredService<IRepository>(),
                    TimeSpan.FromSeconds(5)))
                .AddTransient<ICelebrity, Repository>()
                .AddTransient<ILifeevent, Repository>()
                .AddTransient<ICommon<Celebrity, Lifeevent>, Repository>()
                .BuildServiceProvider();


            // Раздел тестирования:
            Console.WriteLine("=== Тестирование Transient-сервисов ===");

            // Пример 1: Получение всех знаменитостей
            var repo1 = serviceProvider.GetRequiredService<IRepository>();
            var celebs = new[]
               {
                    new Celebrity { FullName = "Edgar Codd", Nationality = "US", ReqPhotoPath = "codd.jpg" },
                    new Celebrity { FullName = "Donald Knuth", Nationality = "XX", ReqPhotoPath = "knuth.jpg" }
                };

            foreach (var c in celebs)
            {
                Console.WriteLine($"Добавление {c.FullName}: {repo1.AddCelebrity(c)}");
            }

            var celebrities = repo1.GetAllCelebrities();

            Console.WriteLine("\nСписок знаменитостей:");
            foreach (var c in celebrities)
            {
                Console.WriteLine($"ID: {c.Id}, Имя: {c.FullName}, Национальность: {c.Nationality}");
            }

            // Пример 2: Добавление новой знаменитости
            var newCelebrity = new Celebrity
            {
                FullName = "Ada Lovelace",
                Nationality = "UK",
                ReqPhotoPath = null
            };

            bool added = repo1.AddCelebrity(newCelebrity);
            Console.WriteLine($"\nЗнаменитость {(added ? "успешно добавлена" : "не добавлена")}");
            
            Console.WriteLine("-----------------------------------------");
            // Пример 3: Получение событий по ID знаменитости
            var repo2 = serviceProvider.GetRequiredService<IRepository>();
            var celebrities2 = repo2.GetAllCelebrities();

            Console.WriteLine("\nСписок знаменитостей:");
            foreach (var c in celebrities2)
            {
                Console.WriteLine($"ID: {c.Id}, Имя: {c.FullName}, Национальность: {c.Nationality}");
            }

            var lifeevent = new Lifeevent
            {
                CelebrityId = 1,
                Date = DateTime.Now,
                Description = "Тестовое событие",
                ReqPhotoPath = "event.jpg"
            };

            Console.WriteLine($"Добавление события: {repo2.AddLifeevent(lifeevent)}");

            Console.WriteLine("\nСписок событий:");
            foreach (var e in repo2.GetAllLifeevents())
            {
                Console.WriteLine($"ID: {e.Id}, Знаменитость: {e.CelebrityId}, Дата: {e.Date.ToShortDateString()}, Описание: {e.Description}");
            }

            // Проверка Transient — каждый Resolve возвращает новый объект
            var repoA = serviceProvider.GetRequiredService<IRepository>();
            var repoB = serviceProvider.GetRequiredService<IRepository>();
            Console.WriteLine($"\nTransient работают корректно? {(repoA != repoB ? "Да" : "Нет")}");

            //кэширование
            Console.WriteLine("\n=== Тестирование Scope-сервисов ===");

            // Создаём scope
            using (var scope = serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICashCelebrities>();

                // Тестируем работу с кэшем
                Console.WriteLine("Получение знаменитостей из кэша...");
                var cachedCelebrities1 = service.GetCachedCelebrities();
                Console.WriteLine($"Первоначальное количество знаменитостей в кэше: {cachedCelebrities1.Count}");

                // Имитация изменения данных через некоторое время
                Console.WriteLine("Ожидаем 6 секунд для обновления кэша...");
                Console.WriteLine($"\nЗнаменитость {(added ? "успешно добавлена" : "не добавлена")}");

                Thread.Sleep(6000); // Ожидаем 6 секунд (больше времени кэширования)

                var cachedCelebrities2 = service.GetCachedCelebrities();
                Console.WriteLine($"Количество знаменитостей в кэше после ожидания: {cachedCelebrities2.Count}");

                // Проверка: кэш должен быть обновлен
                Console.WriteLine($"Кэш обновился? {(cachedCelebrities1 != cachedCelebrities2 ? "Да" : "Нет")}");
            }

            Console.WriteLine("\n=== Тест завершён ===");
        }
    }
}