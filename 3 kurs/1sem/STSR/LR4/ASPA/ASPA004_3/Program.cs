using DAL004.Interfaces;
using DAL004.Repositories;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Repository.JSONFileName = "Celebrities.json";
using (IRepository repository = Repository.Create("Celebrities"))
{
    app.MapGet("/", () => "Hello World!");

    app.UseExceptionHandler("/Celebrities/Error");

    app.MapGet("/Celebrities", () => repository.getAllCelebrities());

    app.MapGet("/Celebrities/{id:int}", (int id) => {
        Celebrity? celebrity = repository.getCelebrityById(id);
        if (celebrity == null) throw new FoundByIdException($"Celebrity Id = {id} not found");
        return celebrity;
    });

    app.MapPost("/Celebrities", (Celebrity celebrity) => {

        int? id = repository.AddCelebrity(celebrity);
        if (id == null) throw new AddCelebrityException("/Celebrities error, id == null");
        if (repository.SaveChanges() <= 0) throw new SaveException("/Celebrities error, SaveChanges() <= 0");
        return new Celebrity((int)id, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath);
    });

    // DELETE
    app.MapDelete("/Celebrities/{id:int}", (int id) => {
        bool success = repository.DelCelebrityById(id);
        if (!success) throw new DeleteCelebrityException($"Celebrity Id = {id} not found or already deleted");
        if (repository.SaveChanges() <= 0) throw new SaveException("/Celebrities error, SaveChanges() <= 0");
        return $"Celebrity Id = {id} deleted";
    });


    // PUT
    app.MapPut("/Celebrities/{id:int}", (int id, Celebrity celebrity) => {
        int? result = repository.UpdCelebrityById(id, celebrity);
        if (result == null) throw new UpdateCelebrityException($"Celebrity Id = {id} not found");
        if (repository.SaveChanges() <= 0) throw new SaveException("/Celebrities error, SaveChanges() <= 0");
        return celebrity;
    });

    app.MapFallback((HttpContext ctx) => Results.NotFound(new { error = $"path {ctx.Request.Path} not supported" }));

    app.Map("/Celebrities/Error", (HttpContext ctx) => {
        Exception? ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
        IResult rc = Results.Problem(detail: "Panic", instance: app.Environment.EnvironmentName, title: "ASPA004", statusCode: 500);

        if (ex != null)
        {
            if (ex is FoundByIdException) rc = Results.NotFound(ex.Message); // не найдено
            if (ex is BadHttpRequestException) rc = Results.BadRequest(ex.Message); // ошибка в формате запроса
            if (ex is SaveException) rc = Results.Problem(title: "ASPA004/SaveChanges", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
            if (ex is AddCelebrityException) rc = Results.Problem(title: "ASPA004/AddCelebrity", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
            else if (ex is DeleteCelebrityException)
                rc = Results.Problem(title: "ASPA004/deleteCelebrity", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
            else if (ex is UpdateCelebrityException)
                rc = Results.Problem(title: "ASPA004/updateCelebrity", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);

        }
        return rc;
    });

    app.Run();
}

public class FoundByIdException : Exception
{
    public FoundByIdException(string message) : base($"Found by Id: {message}") { }
}

public class SaveException : Exception
{
    public SaveException(string message) : base($"SaveChanges error: {message}") { }
}

public class AddCelebrityException : Exception
{
    public AddCelebrityException(string message) : base($"AddCelebrityException error: {message}") { }
}
public class DeleteCelebrityException : Exception
{
    public DeleteCelebrityException(string? message) : base($"DeleteCelebrityException error: {message}") { }
}
public class UpdateCelebrityException : Exception
{
    public UpdateCelebrityException(string? message) : base($"UpdateCelebrityException error: {message}") { }
}