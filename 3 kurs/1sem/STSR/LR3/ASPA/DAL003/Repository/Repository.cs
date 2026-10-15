using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using DAL003.Interfaces;
using DAL003.Models;

namespace DAL003.Repositories
{
    public class Repository : IRepository
    {
        public static string JSONFileName { get; set; } = "Celebrities.json";
        public string BasePath { get; }

        private List<Interfaces.Celebrity> _celebrities;
        private bool _disposed = false;

        // Статический метод Create
        public static IRepository Create(string directoryName)
        {
            var basePath = Path.IsPathRooted(directoryName)
                ? Path.GetFullPath(directoryName)
                : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, directoryName));
            return new Repository(basePath);
        }

        public Repository(string basePath)
        {
            if (string.IsNullOrWhiteSpace(basePath))
                throw new ArgumentException("Base path cannot be null or empty", nameof(basePath));

            BasePath = basePath;

            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }

            LoadCelebrities();
        }

        private void LoadCelebrities()
        {
            var jsonFilePath = Path.Combine(BasePath, JSONFileName);

            if (!File.Exists(jsonFilePath))
            {
                _celebrities = new List<Interfaces.Celebrity>();
                return;
            }

            try
            {
                var jsonString = File.ReadAllText(jsonFilePath);
                // Десериализуем напрямую в Models.Celebrity[]
                var celebrityModels = JsonSerializer.Deserialize<Models.Celebrity[]>(jsonString);

                // Конвертируем Models.Celebrity в Interfaces.Celebrity
                _celebrities = celebrityModels?
                    .Where(c => c != null)
                    .Select(c => new Interfaces.Celebrity(
                        c.Id,
                        c.Firstname ?? string.Empty,
                        c.Surname ?? string.Empty,
                        c.PhotoPath ?? string.Empty
                    ))
                    .ToList() ?? new List<Interfaces.Celebrity>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error loading celebrities from JSON file: {ex.Message}", ex);
            }
        }

        public Interfaces.Celebrity[] getAllCelebrities()
        {
            return _celebrities.ToArray();
        }

        public Interfaces.Celebrity? getCelebrityById(int id)
        {
            return _celebrities.FirstOrDefault(c => c.Id == id);
        }

        public Interfaces.Celebrity[] getCelebritiesBySurname(string surname)
        {
            if (string.IsNullOrWhiteSpace(surname))
                return Array.Empty<Interfaces.Celebrity>();

            return _celebrities
                .Where(c => c.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }

        public string? getPhotoPathById(int id)
        {
            var celebrity = getCelebrityById(id);
            if (string.IsNullOrEmpty(celebrity?.PhotoPath))
                return null;

            // Убираем путь к папке, оставляем только имя файла
            var fileName = Path.GetFileName(celebrity.PhotoPath);

            // Возвращаем URL для доступа к статическому файлу
            return $"/Photo/{fileName}";
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _celebrities?.Clear();
                }
                _disposed = true;
            }
        }
    }
}