using Mango.Contracts.Models.Service;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCart.Contexts
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<ProductGen> Products { get; set; }
        
        public DbSet<CartHeader> CartHeaders { get; set; }

        public DbSet<CartDetail> CartDetails { get; set; }
    }
}
