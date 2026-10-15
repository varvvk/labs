var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles(); // ищет index.html автоматически
// Включаем middleware для обслуживания статических файлов
app.UseStaticFiles();

app.UseWelcomePage("/aspnetcore");
//app.MapGet("/", () => "Hello World!");

app.Run();
