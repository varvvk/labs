using System;
using SericeLocator;

namespace SericeLocator
{
    // Определяем интерфейсы для сервисов с уникальным идентификатором
    public interface ITransientService
    {
        Guid Id { get; }
    }

    public interface ISingletonService
    {
        Guid Id { get; }
    }

    public interface IScopedService
    {
        Guid Id { get; }
    }

    public class TransientService : ITransientService
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    public class SingletonService : ISingletonService
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    public class ScopedService : IScopedService, IDisposable
    {
        public Guid Id { get; } = Guid.NewGuid();

        public void Dispose()
        {
            Console.WriteLine($"ScopedService {Id} disposed");
        }
    }

    class Program
    {
        static void Main()
        {
            // Регистрация сервисов
            TransientLocator.Register<ITransientService>(() => new TransientService());
            SingletonLocator.Register<ISingletonService>(new SingletonService());
            var scopeFactory = ScopeLocator.CreateServiceScopeFactory();
            ScopeLocator.Register<IScopedService>(() => new ScopedService());

            Console.WriteLine("Testing Transient:");
            ITransientService transient1 = TransientLocator.Resolve<ITransientService>();
            ITransientService transient2 = TransientLocator.Resolve<ITransientService>();
            Console.WriteLine($"Transient instances same: {object.ReferenceEquals(transient1, transient2)}");
            Console.WriteLine($"Transient1 ID: {transient1.Id}");
            Console.WriteLine($"Transient2 ID: {transient2.Id}");

            Console.WriteLine("\nTesting Singleton:");
            ISingletonService singleton1 = SingletonLocator.Resolve<ISingletonService>();
            ISingletonService singleton2 = SingletonLocator.Resolve<ISingletonService>();
            Console.WriteLine($"Singleton instances same: {object.ReferenceEquals(singleton1, singleton2)}");
            Console.WriteLine($"Singleton1 ID: {singleton1.Id} \n Singleton2 ID: {singleton2.Id}");

            Console.WriteLine("\nTesting Scoped:");
            using (var scope1 = scopeFactory.CreateScope())
            {
                IScopedService scoped1 = (IScopedService)scope1.ServiceProvider.Resolve(typeof(IScopedService));
                IScopedService scoped2 = (IScopedService)scope1.ServiceProvider.Resolve(typeof(IScopedService));
                Console.WriteLine($"Scoped instances same in same scope: {object.ReferenceEquals(scoped1, scoped2)}");
                Console.WriteLine($"Scoped1 ID: {scoped1.Id} \n Scoped2 ID: {scoped2.Id}");
            }

            using (var scope2 = scopeFactory.CreateScope())
            {
                IScopedService scoped3 = (IScopedService)scope2.ServiceProvider.Resolve(typeof(IScopedService));
                IScopedService scoped4 = (IScopedService)scope2.ServiceProvider.Resolve(typeof(IScopedService));
                Console.WriteLine($"Scoped instances same in different scope: {object.ReferenceEquals(scoped3, scoped4)}");
                Console.WriteLine($"Scoped3 ID: {scoped3.Id} \n Scoped4 ID: {scoped4.Id}");
            }


            SingletonLocator.Reset();
            TransientLocator.Reset();
        }
    }
}