using MedicalCenter.Models;

public interface IClientRepository
{
    Client? GetByLogin(string phoneOrEmail, string password);
}
