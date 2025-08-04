using Reseller.Domain.Data.ValueObjects;
using Reseller.Domain.Entities;

namespace Reseller.Domain.Interfaces.Repositories
{
    public interface IRevendaRepository
    {
        Task<bool> CreateAsync(Revenda revenda);
        Task<Revenda?> GetByIdAsync(Guid Id);
        Task<bool> ExistsByCnpjAsync(CnpjValueObject cnpj);
    }
}