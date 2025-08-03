using Carter;
using FluentValidation;
using MassTransit;
using MediatR;
using OrderService.Contracts;
using OrderService.Database;
using OrderService.Entities;
using OrderService.Enums;
using OrderService.Features;
using Shared;

namespace OrderService.Features
{
    public static class CriarPedido
    {
        public class Command : IRequest<Result<PedidoResponse>>
        {
            public string NomeCliente { get; set; }
            public string TelefoneCliente { get; set; }
            public string Cnpj { get; set; }
            public string Cpf { get; set; }
            public List<CriarPedidoItemModel> Itens { get; set; }
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
                RuleFor(x => x.NomeCliente)
                    .NotEmpty().WithMessage("O nome do cliente é obrigatório.")
                    .MaximumLength(100).WithMessage("O nome do cliente deve ter no máximo 100 caracteres.");

                RuleFor(x => x.TelefoneCliente)
                    .NotEmpty().WithMessage("O telefone do cliente é obrigatório.")
                    .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("O telefone do cliente deve ser um número válido.");

                RuleFor(x => x.Itens)
                    .NotEmpty().WithMessage("O pedido deve conter pelo menos um item.")
                    .Must(itens => itens.All(item => item.Quantidade > 0))
                    .WithMessage("Todos os itens devem ter quantidade maior que zero.")
                    .Must(itens => !itens.Any(Item => string.IsNullOrEmpty(Item.Nome)))
                    .WithMessage("Todos os itens devem conter o nome");

                RuleFor(x => new { x.Cpf, x.Cnpj })
                    .Must(x => !(!Validations.IsValidCpf(x.Cpf) && !Validations.IsValidCnpj(x.Cnpj)))
                    .WithMessage("É necessário informar um CPF ou um CNPJ válido.");
            }
        }

        internal sealed class Handler : IRequestHandler<Command, Result<PedidoResponse>>
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly IValidator<Command> _validator;
            private readonly IPublishEndpoint _publishEndpoint;

            public Handler(ApplicationDbContext dbContext, IValidator<Command> validator, IPublishEndpoint publishEndpoint)
            {
                _dbContext = dbContext;
                _validator = validator;
                _publishEndpoint = publishEndpoint;
            }

            public async Task<Result<PedidoResponse>> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return Result.Failure<PedidoResponse>(new Error("CriarPedido.Validation", validationResult.ToString()));
                }

                var itens = request.Itens.Select(item => new Item
                {
                    Referencia = item.Referencia,
                    Nome = item.Nome,
                    Quantidade = item.Quantidade
                }).ToArray();

                var pedido = new Pedido()
                {
                    Id = Guid.NewGuid(),
                    NomeCliente = request.NomeCliente,
                    TelefoneCliente = request.TelefoneCliente,
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
                Cnpj = request.Cnpj,
                Cpf = request.Cpf,
                NomeCliente = request.NomeCliente,
                TelefoneCliente = request.TelefoneCliente,
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