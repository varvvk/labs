
using System.Collections.Concurrent;


namespace SericeLocator
{
    public static class TransientLocator
    {
        private static readonly ConcurrentDictionary<Type, Func<object>> _services = new();

        public static void Register<T>(Func<T> resolver) where T : class
        {
            _services[typeof(T)] = resolver;//функция
        }

        public static T Resolve<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var resolver))
            {
                return (T)resolver();
            }
            throw new InvalidOperationException($"Сервис {typeof(T)} не зарегистрирован");
        }

        public static void Reset()
        {
            _services.Clear();
        }
    }
}
