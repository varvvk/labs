using Microsoft.AspNetCore.HttpLogging;

internal class Program
{
    // Точка входа приложения
    private static void Main(string[] args)
    {
        // Создаём строитель веб-приложения ASP.NET Core.
        // Сюда автоматически подгружаются конфигурация (appsettings.json), логирование,
        // переменные окружения и др. Параметр args — аргументы командной строки.
        var builder = WebApplication.CreateBuilder(args);

        // Подключаем сервис HTTP Logging
        builder.Services.AddHttpLogging(logging =>
        {
            // Логировать свойства запроса и заголовки
            logging.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders
                                  | HttpLoggingFields.ResponsePropertiesAndHeaders;
        });


        // Строим (создаём) объект приложения на основе настроек builder.
        // После этого можно регистрировать маршруты, middleware и т.д.
        var app = builder.Build();

        // Включаем middleware HTTP Logging
        app.UseHttpLogging();

        // Регистрируем маршрут для GET-запроса к корневому URL ("/").
        // Когда пользователь заходит на http://localhost:порт/, приложение вернёт строку "Мое первое ASPA".
        app.MapGet("/", () => "Мое первое ASPA");

        // Запускаем веб-сервер Kestrel и начинаем обрабатывать входящие HTTP-запросы.
        app.Run();
    }
}
