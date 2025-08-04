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
using OrderService.HttpClients.PedidosClient;
using OrderService.HttpClients.PedidosClient.Models;
using FluentValidation;
using MassTransit;
using Shared;
using Microsoft.EntityFrameworkCore;
using OrderService.HttpClients.RevendaClient.Models;

namespace OrderService.Tests.Features
{
    public class EnviarPedidosHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenValidationFails()
        {
            var validator = new Mock<IValidator<EnviarPedidos.Command>>();
            validator.Setup(v => v.Validate(It.IsAny<EnviarPedidos.Command>()))
                .Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("RevendaId", "ID Revenda é obrigatório.") }));
            var dbContext = new Mock<ApplicationDbContext>();
            var publishEndpoint = new Mock<IPublishEndpoint>();
            var revendaClient = new Mock<RevendaClient>();
            var pedidosClient = new Mock<PedidosClient>();

            var handler = new EnviarPedidos.Handler(dbContext.Object, validator.Object, publishEndpoint.Object, revendaClient.Object, pedidosClient.Object);
            var command = new EnviarPedidos.Command { RevendaId = Guid.Empty };

            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("EnviarPedidos.Validation");
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenRevendaNotFound()
        {
            var validator = new Mock<IValidator<EnviarPedidos.Command>>();
            validator.Setup(v => v.Validate(It.IsAny<EnviarPedidos.Command>()))
                .Returns(new FluentValidation.Results.ValidationResult());
            var dbContext = new Mock<ApplicationDbContext>();
            var publishEndpoint = new Mock<IPublishEndpoint>();
            var revendaClient = new Mock<RevendaClient>();
            var pedidosClient = new Mock<PedidosClient>();

            var handler = new EnviarPedidos.Handler(dbContext.Object, validator.Object, publishEndpoint.Object, revendaClient.Object, pedidosClient.Object);
            var command = new EnviarPedidos.Command { RevendaId = Guid.NewGuid() };

            var result = await handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Error.Code.Should().Be("EnviarPedidos.Revenda");
        }
    }
}
