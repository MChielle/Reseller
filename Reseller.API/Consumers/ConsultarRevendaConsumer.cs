using MassTransit;
using OrderService.Contracts.ConsultarRevenda;
using Reseller.Domain.Interfaces.Services;
using Reseller.Domain.Models.RevendaModels.Get;

namespace Reseller.API.Consumers
{
    public class ConsultarRevendaConsumer : IConsumer<IConsultarRevendaRequest>
    {
        private readonly IRevendaService _revendaService;

        public ConsultarRevendaConsumer(IRevendaService revendaService)
        {
            _revendaService = revendaService;
        }

        public async Task Consume(ConsumeContext<IConsultarRevendaRequest> context)
        {
            var revenda = await _revendaService.GetRevendaByIdAsync(new RevendaGetByIdModelRequest { Id = context.Message.Id });

            var response = new { Existe = revenda != null };

            await context.RespondAsync<IConsultarRevendaResponse>(response);
        }
    }
}