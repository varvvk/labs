using System.Collections.Concurrent;

namespace SericeLocator
{
    public static class SingletonLocator
    {
        private static readonly ConcurrentDictionary<Type, object> _services = new();

        public static void Register<T>(T service) where T : class
        {
            _services[typeof(T)] = service;
        }

        public static T Resolve<T>() where T : class 
        {
            if (_services.TryGetValue(typeof(T), out var service))
            {
                return (T)service;
            }
            throw new InvalidOperationException($"Сервис {typeof(T)} не зарегистрирован");
        }

        public static void Reset()

        {
            _services.Clear();
        }
    }
}
