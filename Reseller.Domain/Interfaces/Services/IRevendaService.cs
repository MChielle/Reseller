using Reseller.Domain.Models.RevendaModels.Get;
using Reseller.Domain.Models.RevendaModels.GetByCnpj;
using Reseller.Domain.Models.RevendaModels.Input;

namespace Reseller.Domain.Interfaces.Services
{
    public interface IRevendaService
    {
        Task<bool> CreateRevendaAsync(RevendaCreateModelRequest request);
        Task<RevendaGetByCnpjModelResponse> GetRevendaByCnpjAsync(RevendaGetByCnpjModelRequest request);
    }
}