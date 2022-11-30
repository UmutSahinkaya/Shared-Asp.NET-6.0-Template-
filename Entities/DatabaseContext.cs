using Microsoft.EntityFrameworkCore;

namespace Shared.Entities
{
    public class DatabaseContext:DbContext 
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
