using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL004.Interfaces
{
    public interface IRepository : IDisposable
    {
        string BasePath { get; } // полный директорий для JSON и фотографий
        Celebrity[] getAllCelebrities(); // получить весь список знаменитостей
        Celebrity? getCelebrityById(int id); // получить знаменитость по Id
        Celebrity[] getCelebritiesBySurname(string surname); // получить знаменитость по фамилии
        string? getPhotoPathById(int id); // получить путь для GET-запроса к фотографии
        int? AddCelebrity(Celebrity celebrity); // Добавить знаменитость, возвращает Id новой знаменитости
        bool DelCelebrityById(int id); // Удалить знаменитость по Id, возвращает true при успехе
        int? UpdCelebrityById(int id, Celebrity celebrity); // Изменить знаменитость по Id, возвращает новый Id при успехе
        int SaveChanges(); //сохранить изменения в Json, возвращать количество изменений
    }

    public record Celebrity(int Id, string Firstname, string Surname, string PhotoPath);
}