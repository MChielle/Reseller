using System;
using System.Collections.Generic;
using FluentAssertions;
using OrderService.Features;
using Xunit;

namespace OrderService.Tests.Features
{
    public class EnviarPedidosValidatorTests
    {
        [Fact]
        public void Should_Fail_When_RevendaId_IsDefault()
        {
            var validator = new EnviarPedidos.Validator();
            var command = new EnviarPedidos.Command { RevendaId = default };
            var result = validator.Validate(command);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Pass_When_RevendaId_IsValid()
        {
            var validator = new EnviarPedidos.Validator();
            var command = new EnviarPedidos.Command { RevendaId = Guid.NewGuid() };
            var result = validator.Validate(command);
            result.IsValid.Should().BeTrue();
        }
    }
}
