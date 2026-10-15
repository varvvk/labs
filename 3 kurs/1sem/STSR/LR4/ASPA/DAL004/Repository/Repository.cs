using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using DAL004.Interfaces;
using DAL004.Models;

namespace DAL004.Repositories
{
    public class Repository : IRepository
    {
        public static string JSONFileName { get; set; } = "Celebrities.json";
        public string BasePath { get; }

        private List<Interfaces.Celebrity> _celebrities;
        private bool _disposed = false;
        private bool _isModified = false;

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

        private void SaveToJson()
        {
            var jsonFilePath = Path.Combine(BasePath, JSONFileName);

            // Конвертируем обратно в Models.Celebrity для сериализации
            var celebrityModels = _celebrities.Select(c => new Models.Celebrity
            {
                Id = c.Id,
                Firstname = c.Firstname,
                Surname = c.Surname,
                PhotoPath = c.PhotoPath
            }).ToArray();

            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonString = JsonSerializer.Serialize(celebrityModels, options);
            File.WriteAllText(jsonFilePath, jsonString);
            _isModified = false;
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

        public int? AddCelebrity(Interfaces.Celebrity celebrity)
        {
            if (celebrity == null)
                throw new ArgumentNullException(nameof(celebrity));

            // Если Id = 0 — не добавляем
            if (celebrity.Id == 0)
                return null;

            // Проверяем, существует ли уже запись с таким Id
            if (_celebrities.Any(c => c.Id == celebrity.Id))
                return null;

            _celebrities.Add(celebrity);
            _isModified = true;
            return celebrity.Id;
        }


        public bool DelCelebrityById(int id)
        {
            var celebrity = getCelebrityById(id);
            if (celebrity == null)
                return false;

            var result = _celebrities.Remove(celebrity);
            if (result)
            {
                _isModified = true;
            }
            return result;
        }

        public int? UpdCelebrityById(int id, Interfaces.Celebrity celebrity)
        {
            if (celebrity == null)
                throw new ArgumentNullException(nameof(celebrity));

            var existingCelebrity = getCelebrityById(id);
            if (existingCelebrity == null)
                return null;

            // Удаляем старую запись и добавляем новую
            _celebrities.Remove(existingCelebrity);
            _celebrities.Add(celebrity);
            _isModified = true;

            return celebrity.Id;
        }

        public int SaveChanges()
        {
            if (!_isModified)
                return 0;

            try
            {
                SaveToJson();
                return 1; // Возвращаем количество сохраненных изменений
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error saving changes to JSON file: {ex.Message}", ex);
            }
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
                    // При dispose сохраняем изменения
                    if (_isModified)
                    {
                        SaveChanges();
                    }
                    _celebrities?.Clear();
                }
                _disposed = true;
            }
        }
    }
}