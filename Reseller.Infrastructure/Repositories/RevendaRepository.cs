using Microsoft.EntityFrameworkCore;
using Reseller.Domain.Interfaces.Repositories;
using Reseller.Domain.Data.ValueObjects;
using Reseller.Domain.Entities;

namespace Reseller.Infrastructure.Repositories
{
    public class RevendaRepository : IRevendaRepository
    {
        private readonly ResellerContext _context;

        public RevendaRepository(ResellerContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Revenda revenda)
        {
            var entityEntry = await _context.AddAsync(revenda);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> ExistsByCnpjAsync(CnpjValueObject cnpj)
        {
            return await _context.Revendas
                .AsNoTracking()
                .AnyAsync(r => r.Cnpj.Equals(cnpj.Value));
        }

        public async Task<Revenda?> GetByIdAsync(Guid Id)
        {
            return await _context.Revendas
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id.Equals(Id));
        }
    }
}