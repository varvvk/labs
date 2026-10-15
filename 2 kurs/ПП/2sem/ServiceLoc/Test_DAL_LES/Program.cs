using DAL_LES;

namespace Test_DAL_LES
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ТЕСТИРОВАНИЕ РЕПОЗИТОРИЯ ===");

            ClearTestData();

            Console.WriteLine("\n=== ОСНОВНЫЕ ОПЕРАЦИИ ===");

            using (var repo = new Repository())
            {
                // Тест 1: Поиск несуществующего события
                Console.WriteLine("\n[Тест] Поиск знаменитости по несуществующему событию:");
                var celeb = repo.GetCelebrityByLifeeventId(999);
                Console.WriteLine(celeb == null ? "Ожидаемый результат: не найдено" : "ОШИБКА: Найдена знаменитость!");

                //Тест 2: Поиск событий у несуществующей знаменитости
                Console.WriteLine("\n[Тест] Поиск ");
                var events = repo.GetLifeeventsByCelebrityId(1);
                foreach (var e in events)
                {
                    Console.WriteLine($"Событие: {e.Description}, Дата: {e.Date.ToShortDateString()}");
                }
                Console.WriteLine(celeb == null ? "Ожидаемый результат: не найдено" : "ОШИБКА: Найдена знаменитость!");


                Console.WriteLine("\n=== ТЕСТ ICelebrity ===");
                // Добавляем тестовые данные
                var celebs = new[]
                {
                    new Celebrity { FullName = "Edgar Codd", Nationality = "US", ReqPhotoPath = "codd.jpg" },
                    new Celebrity { FullName = "Donald Knuth", Nationality = "XX", ReqPhotoPath = "knuth.jpg" }
                };

                foreach (var c in celebs)
                {
                    Console.WriteLine($"Добавление {c.FullName}: {repo.AddCelebrity(c)}");
                }

                // Выводим всех знаменитостей
                Console.WriteLine("\nСписок знаменитостей:");
                foreach (var c in repo.GetAllCelebrities())
                {
                    Console.WriteLine(PrintCelebrity(c));
                }

                Console.WriteLine("\n=== ТЕСТ ILifeevent ===");

                // Сначала создаём знаменитость
                var celeb2 = new Celebrity
                {
                    FullName = "Test Celebrity",
                    Nationality = "Test",
                    ReqPhotoPath = "test.jpg"
                };
                repo.AddCelebrity(celeb2);

                // Добавляем событие
                var lifeevent = new Lifeevent
                {
                    CelebrityId = celeb2.Id,
                    Date = DateTime.Now,
                    Description = "Тестовое событие",
                    ReqPhotoPath = "event.jpg"
                };

                Console.WriteLine($"Добавление события: {repo.AddLifeevent(lifeevent)}");

                // Выводим все события
                Console.WriteLine("\nСписок событий:");
                foreach (var e in repo.GetAllLifeevents())
                {
                    Console.WriteLine($"ID: {e.Id}, Знаменитость: {e.CelebrityId}, Дата: {e.Date.ToShortDateString()}, Описание: {e.Description}");
                }
            }

            Console.WriteLine("\n=== ТЕСТ ЗАВЕРШЁН ===");
            Console.ReadKey();
        }

        static void ClearTestData()
        {
            using (var repo = new Repository())
            {
                foreach (var e in repo.GetAllLifeevents())
                    repo.DelLifeevent(e.Id);

                foreach (var c in repo.GetAllCelebrities())
                    repo.DelCelebrity(c.Id);
            }
        }

        static string PrintCelebrity(Celebrity c)
        {
            return $"{c.Id}: {c.FullName} ({c.Nationality}) [{c.ReqPhotoPath}]";
        }
    }
}