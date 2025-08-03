using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reseller.Domain.Interfaces.Repositories;
using Reseller.Domain.Interfaces.Services;
using Reseller.Infrastructure.Repositories;
using Reseller.Service.Services;

namespace Reseller.Infrastructure
{
    public sealed class InjectionFactory : IDesignTimeDbContextFactory<ResellerContext>
    {
        public static void ConfigureContext(IServiceCollection services, IConfiguration configuration, string defaultConnectionString)
        {
            services.AddDbContext<ResellerContext>(options =>
            {
                options.UseSqlite(defaultConnectionString);
            });

            ConfigureServices(services);
            ConfigureRepositories(services);
        }

        public ResellerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ResellerContext>();
            optionsBuilder.UseSqlite("Data Source=database.dat");
            return new ResellerContext(optionsBuilder.Options);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRevendaService, RevendaService>();
        }

        private static void ConfigureRepositories(IServiceCollection services)
        {
            services.AddTransient<IRevendaRepository, RevendaRepository>();
        }
    }
}