using MedicalCenter;
using MedicalCenter.Models;
using System.Linq;

public class AdminRepository : IAdminRepository
{
    private readonly Context _context;

    public AdminRepository(Context context)
    {
        _context = context;
    }

    public Admin? GetByLogin(string login, string password)
    {
        return _context.Admins.FirstOrDefault(a =>
            a.Login == login && a.Password == password);
    }
}
