using MedicalCenter.Models;

namespace MedicalCenter.Repositories
{
    public interface IServiceRepository
    {
        IEnumerable<Service> GetAll();
        Service GetById(int id);
        void Add(Service service);
        void Update(Service service);
        void Delete(int id);
        Task<IEnumerable<Service>> GetFilteredServicesAsync(string search, string category, string sortOption);
    }
}
