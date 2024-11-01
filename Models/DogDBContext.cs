using Microsoft.EntityFrameworkCore;
namespace CodebridgeTest.Models
{
    public class DogDBContext : DbContext
    {
        public DbSet<Dogs> Dogs { get; set; }

        public DogDBContext(DbContextOptions<DogDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dogs>().HasIndex(d => d.Name).IsUnique();
        }
    }
}
