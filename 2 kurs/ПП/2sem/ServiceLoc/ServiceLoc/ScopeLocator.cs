using System;
using System.Collections.Concurrent;

namespace SericeLocator
{
    public static class ScopeLocator
    {
        private static readonly ConcurrentDictionary<Type, Func<object>> _services = new();

        public static void Register<T>(Func<T> resolver) where T : class
        {
            _services[typeof(T)] = resolver;
        }

        public static IServiceScopeFactory CreateServiceScopeFactory() // Создает объект, управляющий областями видимости сервисов
        {
            return new ServiceScopeFactory(_services);
        }

        private class ServiceScope : IServiceScope//область видимости для сервисов.
        {
            private readonly ConcurrentDictionary<Type, object> _scopedServices = new(); // Хранение экземпляров сервисов в области видимости.
            private readonly ConcurrentDictionary<Type, Func<object>> _services;

            public IServiceProvider ServiceProvider { get; }

            public ServiceScope(ConcurrentDictionary<Type, Func<object>> services)
            {
                _services = new ConcurrentDictionary<Type, Func<object>>(services);
                ServiceProvider = new ServiceProvider(_services, _scopedServices);
            }

            public void Dispose()
            {
                _scopedServices.Clear();
            }
        }

        private class ServiceProvider : IServiceProvider
        {
            private readonly ConcurrentDictionary<Type, Func<object>> _services;
            private readonly ConcurrentDictionary<Type, object> _scopedServices;

            public ServiceProvider(
                ConcurrentDictionary<Type, Func<object>> services,
                ConcurrentDictionary<Type, object> scopedServices)
            {
                _services = services;
                _scopedServices = scopedServices;
            }

            public object? Resolve(Type serviceType)
            {
                if (!_services.TryGetValue(serviceType, out var resolver))
                {
                    return null;
                }

                return _scopedServices.GetOrAdd(serviceType, key => resolver());
            }
        }

        private class ServiceScopeFactory : IServiceScopeFactory
        {
            private readonly ConcurrentDictionary<Type, Func<object>> _services;

            public ServiceScopeFactory(ConcurrentDictionary<Type, Func<object>> services)
            {
                _services = services;
            }

            public IServiceScope CreateScope()
            {
                return new ServiceScope(_services);
            }
        }
    }
}


