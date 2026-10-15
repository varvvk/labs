using System.Text.Json;

namespace DAL005;

public class Repository : IRepository
{
    public static string JSONFileName { get; set; } = string.Empty;
    private readonly string _basePath;
    private readonly List<Celebrity> _celebrities;
    private bool _disposed;
    private int _pendingChanges;

    public string BasePath => _basePath;

    public static IRepository Create(string basePath)
    {
        return new Repository(basePath);
    }

    public Repository(string basePath)
    {
        _basePath = Path.GetFullPath(basePath);
        string jsonFilePath = Path.Combine(_basePath, JSONFileName);

        if (!File.Exists(jsonFilePath))
        {
            throw new FileNotFoundException($"JSON file not found at: {jsonFilePath}");
        }

        string jsonContent = File.ReadAllText(jsonFilePath);
        _celebrities = JsonSerializer.Deserialize<List<Celebrity>>(jsonContent)
            ?? throw new InvalidOperationException("Failed to deserialize JSON data");
        _pendingChanges = 0;
    }

    public Celebrity[] GetAllCelebrities()
    {
        EnsureNotDisposed();
        return _celebrities.ToArray();
    }

    public Celebrity? GetCelebrityById(int id)
    {
        EnsureNotDisposed();
        return _celebrities.FirstOrDefault(c => c.Id == id);
    }

    public Celebrity[] GetCelebritiesBySurname(string surname)
    {
        EnsureNotDisposed();
        if (string.IsNullOrWhiteSpace(surname))
            return Array.Empty<Celebrity>();

        return _celebrities
            .Where(c => c.Surname != null && c.Surname.Equals(surname, StringComparison.OrdinalIgnoreCase))
            .ToArray();
    }

    public string? GetPhotoPathById(int id)
    {
        EnsureNotDisposed();
        var celebrity = _celebrities.FirstOrDefault(c => c.Id == id);
        if (celebrity == null)
            return null;

        string fullPath = Path.Combine(_basePath, celebrity.PhotoPath.TrimStart('/'));
        return File.Exists(fullPath) ? celebrity.PhotoPath : null;
    }

    public int? AddCelebrity(Celebrity celebrity)
    {
        EnsureNotDisposed();
        int newId = _celebrities.Count == 0 ? 1 : _celebrities.Max(c => c.Id) + 1;
        var newCelebrity = celebrity with { Id = newId };
        _celebrities.Add(newCelebrity);
        _pendingChanges++;
        return newId;
    }

    public bool DeleteCelebrityById(int id)
    {
        EnsureNotDisposed();
        var index = _celebrities.FindIndex(c => c.Id == id);
        if (index < 0) return false;
        _celebrities.RemoveAt(index);
        _pendingChanges++;
        return true;
    }

    public bool UpdateCelebrityById(int id, Celebrity celebrity)
    {
        EnsureNotDisposed();
        var idx = _celebrities.FindIndex(c => c.Id == id);
        if (idx < 0) return false;
        _celebrities[idx] = celebrity with { Id = id };
        _pendingChanges++;
        return true;
    }

    public int SaveChanges()
    {
        EnsureNotDisposed();
        string jsonFilePath = Path.Combine(_basePath, JSONFileName);
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(_celebrities, options);
        File.WriteAllText(jsonFilePath, json);
        int saved = _pendingChanges;
        _pendingChanges = 0;
        return saved;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _disposed = true;
        }
    }

    private void EnsureNotDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(Repository));
    }
}