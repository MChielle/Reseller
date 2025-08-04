using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using OrderService.Features;
using OrderService.Database;
using OrderService.Entities;
using OrderService.Enums;
using OrderService.HttpClients.Revenda;
using FluentValidation;
using MassTransit;
using Shared;
using OrderService.HttpClients.RevendaClient.Models;

namespace OrderService.Tests.Features
{
    public class CriarPedidoHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenValidationFails()
        {
            var validator = new Mock<IValidator<CriarPedido.Command>>();
            validator.Setup(v => v.Validate(It.IsAny<CriarPedido.Command>()))
                .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Cliente.Nome", "Nome obrigatório") }));
            var dbContext = new Mock<ApplicationDbContext>();
            var publishEndpoint = new Mock<IPublishEndpoint>();
            var revendaClient = new Mock<RevendaClient>(null as HttpClient);

            var handler = new CriarPedido.Handler(dbContext.Object, validator.Object, publishEndpoint.Object, revendaClient.Object);
            var command = new CriarPedido.Command
            {
                RevendaId = Guid.NewGuid(),
                Cliente = new CriarPedido.CriarPedidoClienteModel(),
                Itens = new List<CriarPedido.CriarPedidoItemModel>()
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("CriarPedido.Validation");
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenRevendaNotFound()
        {
            var validator = new Mock<IValidator<CriarPedido.Command>>();
            validator.Setup(v => v.Validate(It.IsAny<CriarPedido.Command>()))
                .Returns(new FluentValidation.Results.ValidationResult());
            var dbContext = new Mock<ApplicationDbContext>();
            var publishEndpoint = new Mock<IPublishEndpoint>();
            var revendaClient = new Mock<IRevendaClient>();

            var handler = new CriarPedido.Handler(dbContext.Object, validator.Object, publishEndpoint.Object, revendaClient.Object);
            var command = new CriarPedido.Command
            {
                RevendaId = Guid.NewGuid(),
                Cliente = new CriarPedido.CriarPedidoClienteModel { Nome = "Cliente", Telefone = "+5511999999999" },
                Itens = new List<CriarPedido.CriarPedidoItemModel> { new CriarPedido.CriarPedidoItemModel { Referencia = Guid.NewGuid(), Nome = "Produto", Quantidade = 1 } }
            };

            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("CriarPedido.Revenda");
        }
    }
}
