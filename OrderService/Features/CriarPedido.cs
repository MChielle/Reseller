using Carter;
using FluentValidation;
using MassTransit;
using MediatR;
using OrderService.Contracts.CriarPedido;
using OrderService.Contracts.EnviarPedidos;
using OrderService.Database;
using OrderService.Entities;
using OrderService.Enums;
using OrderService.Features;
using OrderService.HttpClients.Revenda;
using Shared;

namespace OrderService.Features
{
    public static class CriarPedido
    {
        public class Command : IRequest<Result<PedidoResponse>>
        {
            public Guid RevendaId { get; set; }
            public CriarPedidoClienteModel Cliente { get; set; }
            public List<CriarPedidoItemModel> Itens { get; set; }
        }

        public sealed class CriarPedidoClienteModel
        {
            public string Cpf { get; set; }
            public string Cnpj { get; set; }
            public string Nome { get; set; }
            public string Telefone { get; set; }
        }

        public sealed class CriarPedidoItemModel
        {
            public Guid Referencia { get; set; }
            public string Nome { get; set; }
            public int Quantidade { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.RevendaId)
                    .NotEmpty().WithMessage("O ID da revenda é obrigatório.");

                RuleFor(x => x.Cliente.Nome)
                    .NotEmpty().WithMessage("O nome do cliente é obrigatório.")
                    .MaximumLength(100).WithMessage("O nome do cliente deve ter no máximo 100 caracteres.");

                RuleFor(x => x.Cliente.Telefone)
                    .NotEmpty().WithMessage("O telefone do cliente é obrigatório.")
                    .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("O telefone do cliente deve ser um número válido.");

                RuleFor(x => new { x.Cliente.Cpf, x.Cliente.Cnpj })
                    .Must(x => !(!Validations.IsValidCpf(x.Cpf) && !Validations.IsValidCnpj(x.Cnpj)))
                    .WithMessage("É necessário informar um CPF ou um CNPJ válido.");

                RuleFor(x => x.Itens)
                    .NotEmpty().WithMessage("O pedido deve conter pelo menos um item.")
                    .Must(itens => itens.All(item => item.Quantidade > 0))
                    .WithMessage("Todos os itens devem ter quantidade maior que zero.")
                    .Must(itens => !itens.Any(Item => string.IsNullOrEmpty(Item.Nome)))
                    .WithMessage("Todos os itens devem conter o nome");
            }
        }

        public sealed class Handler : IRequestHandler<Command, Result<PedidoResponse>>
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly IValidator<Command> _validator;
            private readonly IPublishEndpoint _publishEndpoint;
            private RevendaClient _revendaClient;

            public Handler(ApplicationDbContext dbContext, IValidator<Command> validator, IPublishEndpoint publishEndpoint, RevendaClient revendaClient)
            {
                _dbContext = dbContext;
                _validator = validator;
                _publishEndpoint = publishEndpoint;
                _revendaClient = revendaClient;
            }

            public async Task<Result<PedidoResponse>> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return Result.Failure<PedidoResponse>(new Error("CriarPedido.Validation", validationResult.ToString()));
                }

                var revenda = await _revendaClient.GetRevendaByIdAsync(request.RevendaId);

                if (revenda == null)
                {
                    return Result.Failure<PedidoResponse>(new Error("CriarPedido.Revenda", "Revenda não encontrada."));
                }

                var itens = request.Itens.Select(item => new Item
                {
                    Referencia = item.Referencia,
                    Nome = item.Nome,
                    Quantidade = item.Quantidade
                }).ToArray();

                var cliente = new Cliente
                {
                    Cnpj = request.Cliente.Cnpj,
                    Cpf = request.Cliente.Cpf,
                    Nome = request.Cliente.Nome,
                    Telefone = request.Cliente.Telefone
                };

                var pedido = new Pedido()
                {
                    Id = Guid.NewGuid(),
                    RevendaId = request.RevendaId,
                    Cliente = cliente,
                    DataPedidoUTC = DateTime.UtcNow,
                    Status = StatusPedido.Criado,
                    Itens = itens
                };

                var response = await _dbContext.Pedidos.AddAsync(pedido);

                await _dbContext.SaveChangesAsync(cancellationToken);

                var result = new PedidoResponse
                {
                    PedidoId = response.Entity.Id,
                    Itens = response.Entity.Itens.Select(item => new CriarPedidoItemResponse
                    {
                        Id = item.Id,
                        Nome = item.Nome,
                        Quantidade = item.Quantidade
                    }).ToArray()
                };

                return result;
            }
        }
    }
}

public class CriarPedidoEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/pedido", async (CriarPedidoRequest request, ISender sender) =>
        {
            var command = new CriarPedido.Command()
            {
                RevendaId = request.RevendaId,
                Cliente = new CriarPedido.CriarPedidoClienteModel
                {
                    Cnpj = request.Cliente.Cnpj,
                    Cpf = request.Cliente.Cpf,
                    Nome = request.Cliente.Nome,
                    Telefone = request.Cliente.Telefone,
                },
                Itens = request.Itens.Select(item => new CriarPedido.CriarPedidoItemModel
                {
                    Referencia = item.Referencia,
                    Nome = item.Nome,
                    Quantidade = item.Quantidade
                }).ToList()
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