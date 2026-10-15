using DAL005;

namespace Validation
{
    public class SurnameFilter : IEndpointFilter
    {
        public static IRepository? repository;

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            // если параметр celebrity = null, то 500 с пояснениями
            // если Surname = null или его длина < 2, то 409 с появнениями
            // если уже есть такая Surname, то 409 с появнениями
            var argument = context.Arguments.OfType<Celebrity>().FirstOrDefault();

            if (argument == null)
            {
                throw new NullCelebrityException("Celebrity parameter is null");
            }

            if (string.IsNullOrWhiteSpace(argument.Surname) || argument.Surname.Length < 2)
            {
                throw new InvalidSurnameException("POST /Celebrities error, Surname is wrong");
            }

            if (repository != null && !string.IsNullOrWhiteSpace(argument.Surname))
            {
                var existingCelebrities = repository.GetCelebritiesBySurname(argument.Surname);
                if (existingCelebrities.Length > 0)
                {
                    throw new DuplicateSurnameException("POST /Celebrities error, Surname is doubled");
                }
            }

            return await next(context);
        }
    }

    public class PhotoExistFilter : IEndpointFilter
    {
        public static IRepository? repository;

        // Добавьте поле для хранения пути к папке с фото
        public static string? PhotosRootPath { get; set; }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var argument = context.Arguments.OfType<Celebrity>().FirstOrDefault();

            if (argument == null)
            {
                throw new NullCelebrityException("Celebrity parameter is null");
            }

            string? fileNameToCheck = null;
            if (!string.IsNullOrWhiteSpace(argument.PhotoPath))
            {
                string fileName = Path.GetFileName(argument.PhotoPath);

                // Определяем путь к файлу фото
                string filePath;
                if (!string.IsNullOrEmpty(PhotosRootPath))
                {
                    // Используем PhotosRootPath, если он задан
                    filePath = Path.Combine(PhotosRootPath, fileName);
                }
                else if (repository != null)
                {
                    // Иначе используем BasePath как в оригинальном коде
                    filePath = Path.Combine(repository.BasePath, fileName);
                }
                else
                {
                    // Если нет ни PhotosRootPath, ни repository, используем текущую директорию
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), "Celebrities", "Photo", fileName);
                }

                if (!File.Exists(filePath))
                {
                    fileNameToCheck = fileName;
                }
            }

            if (fileNameToCheck != null)
            {
                context.HttpContext.Response.Headers.Append("X-Celebrity", $"NotFound={fileNameToCheck}");
            }

            return await next(context);
        }
    }

    public class UpdateCelebrityFilter : IEndpointFilter
    {
        public static IRepository? repository;

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var argument = context.Arguments.OfType<Celebrity>().FirstOrDefault();

            if (argument == null)
            {
                throw new NullCelebrityException("Celebrity parameter is null");
            }

            if (string.IsNullOrWhiteSpace(argument.Surname) || argument.Surname.Length < 2)
            {
                throw new InvalidSurnameException("PUT /Celebrities error, Surname is wrong");
            }

            return await next(context);
        }
    }

    public class DeleteCelebrityFilter : IEndpointFilter
    {
        public static IRepository? repository;

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var idArgument = context.Arguments.OfType<int>().FirstOrDefault();

            if (repository != null && idArgument > 0)
            {
                var celebrity = repository.GetCelebrityById(idArgument);
                if (celebrity == null)
                {
                    throw new DeleteCelebrityException($"Celebrity Id = {idArgument} not found");
                }
            }

            return await next(context);
        }
    }
}