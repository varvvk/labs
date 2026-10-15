using MedicalCenter.Models;

public interface IAdminRepository
{
    Admin? GetByLogin(string login, string password);
}
