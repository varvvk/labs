using MedicalCenter;
using MedicalCenter.Models;
using System.Linq;

public class ClientRepository : IClientRepository
{
    private readonly Context _context;

    public ClientRepository(Context context)
    {
        _context = context;
    }

    public Client? GetByLogin(string phoneOrEmail, string password)
    {
        return _context.Clients.FirstOrDefault(c =>
            (c.Phone == phoneOrEmail) &&
            c.Password == password);
    }
}
