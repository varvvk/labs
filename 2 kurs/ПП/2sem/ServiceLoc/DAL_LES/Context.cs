using Microsoft.EntityFrameworkCore;


namespace DAL_LES
{
    public class Context : DbContext
    {
        public Context() : base()
        { 
            Database.EnsureCreated();
        }
        public DbSet<Celebrity>? Celebrities { get; set; }
        public DbSet<Lifeevent>? Lifeevents { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost; Database=DAL_LES; TrustServerCertificate=True; Integrated Security=True;");
        }
    }

}
