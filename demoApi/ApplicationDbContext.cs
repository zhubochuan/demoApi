using demoApi.Model;
using Microsoft.EntityFrameworkCore;

namespace demoApi
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { 
        }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User() { 
                    Id = 1,
                    Name = "Tom",
                    Password="123"
                }
                );
        }
    }
}
