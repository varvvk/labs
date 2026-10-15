using DAL005;
using Microsoft.AspNetCore.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders; 

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var photoRoot = Path.Combine(builder.Environment.ContentRootPath, "Celebrities", "Photo");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(photoRoot),
    RequestPath = "/Photo"
});

Repository.JSONFileName = "Celebrities.json";

using (IRepository repository = Repository.Create("Celebrities"))
{
    app.UseExceptionHandler("/Celebrities/Error");

    RouteGroupBuilder api = app.MapGroup("/Celebrities");

    Validation.SurnameFilter.repository = Validation.PhotoExistFilter.repository = 
        Validation.UpdateCelebrityFilter.repository = Validation.DeleteCelebrityFilter.repository = repository;

    Validation.PhotoExistFilter.PhotosRootPath = photoRoot;

    api.MapGet("", () => repository.GetAllCelebrities());

    api.MapGet("/{id:int}", (int id) =>
    {
        Celebrity? celebrity = repository.GetCelebrityById(id);
        if (celebrity == null) throw new FoundByIdException($"Celebrity Id = {id}");
        return celebrity;
    });

    api.MapPost("", (Celebrity celebrity) =>
    {
        int? id = repository.AddCelebrity(celebrity);
        if (id == null)
            throw new AddCelebrityException("POST /Celebrities error, id = null");
        if (repository.SaveChanges() <= 0)
            throw new SaveException("/Celebrities error, SaveChanges() <= 0");
        return new Celebrity((int)id, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath);
    })
    .AddEndpointFilter<Validation.SurnameFilter>()
    .AddEndpointFilter<Validation.PhotoExistFilter>();

    api.MapDelete("/{id:int}", (int id) =>
    {
        if (!repository.DeleteCelebrityById(id))
        {
            throw new DeleteCelebrityException($"Celebrity Id = {id} not found or already deleted");
        }

        if (repository.SaveChanges() <= 0)
        {
            throw new SaveException($"/Celebrities/{id} error, SaveChanges() <= 0");
        }

        return Results.Ok($"Celebrity with Id = {id} deleted");
    })
    .AddEndpointFilter<Validation.DeleteCelebrityFilter>();

    api.MapPut("/{id:int}", (int id, Celebrity celebrity) =>
    {
        if (celebrity.Id != id && celebrity.Id != 0)
        {
            throw new BadHttpRequestException($"Id in path ({id}) does not match Id in body ({celebrity.Id})");
        }

        if (!repository.UpdateCelebrityById(id, celebrity))
        {
            throw new UpdateCelebrityException($"Celebrity Id = {id} not found");
        }

        if (repository.SaveChanges() <= 0)
        {
            throw new SaveException($"/Celebrities/{id} error, SaveChanges() <= 0");
        }

        return Results.Ok(new Celebrity(id, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath));
    })
    .AddEndpointFilter<Validation.UpdateCelebrityFilter>();

    app.MapFallback((HttpContext ctx) => Results.NotFound(new { error = $"path {ctx.Request.Path} not supported" }));

    app.Map("/Celebrities/Error", (HttpContext ctx) =>
    {
        Exception? ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;

        IResult rc = Results.Problem(detail: "Panic", instance: app.Environment.EnvironmentName, title: "ASPA004", statusCode: 500);

        if (ex != null)
        {
            if (ex is FileNotFoundException) 
                rc = Results.Problem(title: "ASPA00", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
            else if (ex is FoundByIdException) 
                rc = Results.NotFound(ex.Message); 
            else if (ex is BadRequestException) 
                rc = Results.BadRequest(ex.Message); 
            else if (ex is SaveException) 
                rc = Results.Problem(title: "ASPA004/SaveChanges", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
            else if (ex is AddCelebrityException) 
                rc = Results.Problem(title: "ASPA004/AddCelebrity", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
            else if (ex is DeleteCelebrityException)
                rc = Results.Problem(title: "ASPA004/deleteCelebrity", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
            else if (ex is UpdateCelebrityException) 
                rc = Results.Problem(title: "ASPA004/updateCelebrity", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
            else if (ex is NullCelebrityException)
                rc = Results.Problem(title: "ASPA005/NullCelebrity", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
            else if (ex is InvalidSurnameException)
                rc = Results.Conflict($"Value:{ex.Message}");
            else if (ex is DuplicateSurnameException)
                rc = Results.Conflict($"Value:{ex.Message}");
        }

        return rc;
    });
    app.Run();
}

public class FoundByIdException : Exception 
{ 
    public FoundByIdException(string message) : base($"Found by Id: {message}") { } 
}

public class BadRequestException : Exception 
{ 
    public BadRequestException(string message) : base($"BadRequest error: {message}") { } 
}

public class SaveException : Exception 
{ 
    public SaveException(string message) : base($"SaveChanges error: {message}") { } 
}

public class AddCelebrityException : Exception 
{ 
    public AddCelebrityException(string message) : base($"AddCelebrity exception error: {message}") { } 
}

public class DeleteCelebrityException : Exception
{
    public DeleteCelebrityException(string? message) : base($"DeleteCelebrityException error: {message}") { }
}
public class UpdateCelebrityException : Exception
{
    public UpdateCelebrityException(string? message) : base($"UpdateCelebrityException error: {message}") { }
}

public class NullCelebrityException : Exception
{
    public NullCelebrityException(string message) : base($"NullCelebrityException error: {message}") { }
}

public class InvalidSurnameException : Exception
{
    public InvalidSurnameException(string message) : base($"InvalidSurnameException error: {message}") { }
}

public class DuplicateSurnameException : Exception
{
    public DuplicateSurnameException(string message) : base($"DuplicateSurnameException error: {message}") { }
}
