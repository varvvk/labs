using Microsoft.EntityFrameworkCore;

namespace DALMSQLXG
{
    public class Context : DbContext
    {
        public DbSet<WSRef> WSRefs { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public Context() : base()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost; Database=XG; TrustServerCertificate=True; Integrated Security=True;");
        }
    }
}
