using Carter;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Contracts.EnviarPedidos;
using OrderService.Database;
using OrderService.Enums;
using OrderService.Features;
using OrderService.HttpClients.PedidosClient;
using OrderService.HttpClients.PedidosClient.Models;
using OrderService.HttpClients.Revenda;
using Shared;

namespace OrderService.Features
{
    public static class EnviarPedidos
    {
        public class Command : IRequest<Result<EnviarPedidosResponse>>
        {
            public Guid RevendaId { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.RevendaId)
                    .Must(x => !x.Equals(default))
                    .WithMessage("ID Revenda é obrigatório.");
            }
        }

        public sealed class Handler : IRequestHandler<Command, Result<EnviarPedidosResponse>>
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly IValidator<Command> _validator;
            private readonly IPublishEndpoint _publishEndpoint;
            private RevendaClient _revendaClient;
            private PedidosClient _pedidosClient;

            public Handler(ApplicationDbContext dbContext, IValidator<Command> validator, IPublishEndpoint publishEndpoint, RevendaClient revendaClient, PedidosClient pedidosClient)
            {
                _dbContext = dbContext;
                _validator = validator;
                _publishEndpoint = publishEndpoint;
                _revendaClient = revendaClient;
                _pedidosClient = pedidosClient;
            }

            public async Task<Result<EnviarPedidosResponse>> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return Result.Failure<EnviarPedidosResponse>(new Error("EnviarPedidos.Validation", validationResult.ToString()));
                }

                var revenda = await _revendaClient.GetRevendaByIdAsync(request.RevendaId);

                if (revenda == null)
                {
                    return Result.Failure<EnviarPedidosResponse>(new Error("EnviarPedidos.Revenda", "Revenda não encontrada."));
                }

                var totalItensPedidos = await _dbContext.Pedidos
                                            .Include(x => x.Itens)
                                            .Where(x => x.RevendaId.Equals(request.RevendaId) && x.Status.Equals(StatusPedido.Criado))
                                            .SumAsync(x => x.Itens.Sum(y => y.Quantidade), cancellationToken);

                if(totalItensPedidos < 1000)
                {
                    return Result.Failure<EnviarPedidosResponse>(new Error("EnviarPedidos.Limite", $"Pedido mínimo deve conter 1000 itens, quantidade atual {totalItensPedidos}."));
                }

                var pedidos = await _dbContext.Pedidos
                    .Include(x => x.Itens)
                    .Where(x => x.RevendaId.Equals(request.RevendaId) && x.Status.Equals(StatusPedido.Criado))
                    .ToListAsync(cancellationToken);

                var enviadoComSucesso = await _pedidosClient.EnviarPedidosAsync(new EnviarPedidosRequestModel
                {
                    RevendaId = request.RevendaId,
                    Pedidos = pedidos.Select(p => new EnviarPedidosPedidoModel
                    {
                        RevendaId = p.RevendaId,
                        DataPedidoUTC = p.DataPedidoUTC,
                        Itens = p.Itens.Select(i => new EnviarPedidosPedidoItemModel
                        {
                            Nome = i.Nome,
                            Referencia = i.Referencia,
                            Quantidade = i.Quantidade
                        }).ToList()
                    }).ToList()
                });

                if(!enviadoComSucesso.IsSuccess)
                {
                    return Result.Failure<EnviarPedidosResponse>(new Error("EnviarPedidos.Pedidos", "Falha ao enviar pedidos."));
                }

                pedidos.ForEach(p => p.Status = StatusPedido.Enviado);
                await _dbContext.SaveChangesAsync(cancellationToken);


                return new EnviarPedidosResponse
                {
                    CodigoPedido = enviadoComSucesso.Value
                };
            }
        }
    }
}

public class EnviarPedidosEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/enviar-pedidos", async (EnviarPedidosRequest request, ISender sender) =>
        {
            var command = new EnviarPedidos.Command()
            {
                RevendaId = request.RevendaId
            };

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok(result.Value);
        });
    }
}