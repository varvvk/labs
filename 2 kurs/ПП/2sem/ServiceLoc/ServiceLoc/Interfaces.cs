
namespace SericeLocator
{
    public interface IServiceProvider //предоставляет нужный сервис по его типу
    {
        object? Resolve(Type serviceType);
    }

    public interface IServiceScope : IDisposable//для области видимости
    {
        IServiceProvider ServiceProvider { get; }
    }

    public interface IServiceScopeFactory // Интерфейс для фабрики, которая может создавать новые области
    {
        IServiceScope CreateScope();
    }
}
