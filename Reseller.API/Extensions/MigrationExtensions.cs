using Microsoft.EntityFrameworkCore;
using Reseller.Infrastructure;

namespace Reseller.API.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ResellerContext>();

            dbContext.Database.Migrate();
        }
    }
}
