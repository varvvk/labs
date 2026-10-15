using Microsoft.EntityFrameworkCore;


namespace DBLevel
{
    public class Context : DbContext
    {
        public Context() : base()
        { 
            Database.EnsureCreated();
        }
        public DbSet<WSRef>? WSRefs { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost; Database=SSSS; TrustServerCertificate=True; Integrated Security=True;");
        }
    }

}

//инициализатор для создания готовых строк в базе данных, чтобы таблица была не пустая,
//фреймворк не создаст бд пока мы не будем делать операцию