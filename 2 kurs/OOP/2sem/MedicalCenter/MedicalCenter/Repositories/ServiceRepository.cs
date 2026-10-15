using MedicalCenter.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalCenter.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly Context _context;

        public ServiceRepository()
        {
            _context = new Context(); // Or use Dependency Injection
        }

        public IEnumerable<Service> GetAll() => _context.Services.AsNoTracking().ToList();

        public Service GetById(int id) => _context.Services.Find(id);

        public void Add(Service service)
        {
            _context.Services.Add(service);
            _context.SaveChanges();
        }

        public void Update(Service service)
        {
            _context.Services.Update(service);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var service = _context.Services.Find(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                _context.SaveChanges();
            }
        }

        public async Task<IEnumerable<Service>> GetFilteredServicesAsync(string search, string category, string sortOption)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var query = _context.Services.AsQueryable();

                // Фильтрация по поиску
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(s => EF.Functions.Like(s.Title, $"%{search}%"));
                }

                // Фильтрация по категории
                if (!string.IsNullOrWhiteSpace(category) && category != "All")
                {
                    query = query.Where(s => s.Category == category);
                }

                // Сортировка
                query = sortOption switch
                {
                    "Price" => query.OrderBy(s => s.Price),
                    "Name" or _ => query.OrderBy(s => s.Title)
                };

                // Выполняем запрос асинхронно
                var result = await query.AsNoTracking().ToListAsync();

                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }



    }
}
