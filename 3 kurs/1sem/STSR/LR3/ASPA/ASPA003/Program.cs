using DAL003.Interfaces;
using DAL003.Repositories;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Repository.JSONFileName = "Celebrities.json";

// Используем путь к выходной директории где лежит приложение
var appBasePath = AppContext.BaseDirectory;
var photosPath = Path.Combine(appBasePath, "Celebrities", "Photo");

// Настройка статических файлов для доступа к фотографиям по короткому URL (просмотр)
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(photosPath),
    RequestPath = "/Photo"
});

// Настройка статических файлов для скачивания
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(photosPath),
    RequestPath = "/Celebrities/download",
    OnPrepareResponse = ctx =>
    {
        // Устанавливаем заголовок для скачивания файла
        var fileName = Path.GetFileName(ctx.Context.Request.Path);
        if (!string.IsNullOrEmpty(fileName))
        {
            ctx.Context.Response.Headers.Append("Content-Disposition", $"attachment; filename=\"{fileName}\"");
        }

        // Кэширование на 1 час
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=3600");
    }
});

// Настройка просмотра директории
app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = new PhysicalFileProvider(photosPath),
    RequestPath = "/Celebrities/download"
});

using (IRepository repository = Repository.Create("Celebrities"))
{
    app.MapGet("/Celebrities", () => repository.getAllCelebrities());
    app.MapGet("/Celebrities/{id:int}", (int id) => repository.getCelebrityById(id));
    app.MapGet("/Celebrities/BySurname/{surname}", (string surname) => repository.getCelebritiesBySurname(surname));
    app.MapGet("/Celebrities/PhotoPathById/{id:int}", (int id) => repository.getPhotoPathById(id));
    app.MapGet("/", () => "Hello World!");

    app.Run();
}