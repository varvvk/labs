using Microsoft.EntityFrameworkCore;
using REPO;
using System.Collections.Generic;

namespace DALMSQLX
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
            optionsBuilder.UseSqlServer(@"Server=localhost; Database=SSSS; TrustServerCertificate=True; Integrated Security=True;");
        }
    }
}
