using Microsoft.EntityFrameworkCore;
using OrderService.Entities;

namespace OrderService.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Item> Itens { get; set; }

        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public ApplicationDbContext()
        {            
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new Exception("Database configuration not found");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
