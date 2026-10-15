using DAL_LES;

namespace Test_SL_LES
{
    public interface ICashCelebrities// Интерфейс, который определяет метод для получения списка знаменитостей из кэша
    {
        List<Celebrity> GetCachedCelebrities();
    }

    public class CashCelebrities : ICashCelebrities
    {
        private readonly IRepository _repository; // Репозиторий для получения данных о знаменитостях
        private List<Celebrity> _cache;
        private DateTime _lastUpdate;
        private readonly TimeSpan _cacheDuration;

        public CashCelebrities(IRepository repository, TimeSpan cacheDuration)
        {
            _repository = repository;
            _cacheDuration = cacheDuration;
            Console.WriteLine($"Кэш создан с длительностью: {cacheDuration.TotalSeconds} секунд");
            UpdateCache();
        }

        public List<Celebrity> GetCachedCelebrities()
        {
            Console.WriteLine($"\nПроверка кэша (последнее обновление: {_lastUpdate:HH:mm:ss}, текущее время: {DateTime.Now:HH:mm:ss})");

            if (DateTime.Now - _lastUpdate > _cacheDuration)
            {
                Console.WriteLine($"Кэш устарел (прошло {(DateTime.Now - _lastUpdate).TotalSeconds} сек. > {_cacheDuration.TotalSeconds} сек.)");
                UpdateCache();
            }
            else
            {
                Console.WriteLine($"Кэш актуален (прошло {(DateTime.Now - _lastUpdate).TotalSeconds} сек. <= {_cacheDuration.TotalSeconds} сек.)");
            }

            Console.WriteLine($"Текущее количество записей в кэше: {_cache.Count}");
            return _cache;
        }

        private void UpdateCache()
        {
            Console.WriteLine("Обновление кэша...");
            _cache = _repository.GetAllCelebrities();
            _lastUpdate = DateTime.Now;
            Console.WriteLine($"Кэш обновлен в {_lastUpdate:HH:mm:ss}. Записей: {_cache.Count}");
        }
    }
}
