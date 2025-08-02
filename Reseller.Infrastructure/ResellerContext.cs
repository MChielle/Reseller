using Microsoft.EntityFrameworkCore;
using Reseller.Domain.Data;
using Reseller.Domain.Data.ValueObjects;
using Reseller.Domain.Entities;

namespace Reseller.Infrastructure
{
    public sealed partial class ResellerContext : DbContext
    {
        public DbSet<Revenda> Revendas { get; set; }
        public DbSet<Telefone> Telefones { get; set; }
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }

        public ResellerContext(DbContextOptions options) : base(options)
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