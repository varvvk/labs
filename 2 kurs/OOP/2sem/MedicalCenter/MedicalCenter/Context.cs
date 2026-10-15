using MedicalCenter.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace MedicalCenter
{
    public class Context : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MedCenterConnection"].ConnectionString;

            optionsBuilder.UseSqlServer(connectionString)
                          .EnableSensitiveDataLogging()
                          .EnableDetailedErrors();
        }
    }
}
