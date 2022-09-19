using Mango.Contracts.DBOperations;
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
            optionsBuilder.UseSqlServer(DBConnection.GetConnectionString());
        }

        public DbSet<ProductGen> Products { get; set; }
        
        public DbSet<CartHeader> CartHeaders { get; set; }

        public DbSet<CartDetail> CartDetails { get; set; }
    }
}
