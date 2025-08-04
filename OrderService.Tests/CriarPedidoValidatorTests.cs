using System;
using System.Collections.Generic;
using FluentAssertions;
using OrderService.Features;
using Xunit;

namespace OrderService.Tests.Features
{
    public class CriarPedidoValidatorTests
    {
        [Fact]
        public void Should_Fail_When_ClienteNome_IsEmpty()
        {
            var validator = new CriarPedido.Validator();
            var command = new CriarPedido.Command
            {
                RevendaId = Guid.NewGuid(),
                Cliente = new CriarPedido.CriarPedidoClienteModel { Nome = "", Telefone = "+5511999999999", Cpf = "123", Cnpj = "" },
                Itens = new List<CriarPedido.CriarPedidoItemModel> { new CriarPedido.CriarPedidoItemModel { Referencia = Guid.NewGuid(), Nome = "Produto", Quantidade = 1 } }
            };
            var result = validator.Validate(command);
            result.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Should_Fail_When_Itens_IsEmpty()
        {
            var validator = new CriarPedido.Validator();
            var command = new CriarPedido.Command
            {
                RevendaId = Guid.NewGuid(),
                Cliente = new CriarPedido.CriarPedidoClienteModel { Nome = "Cliente", Telefone = "+5511999999999", Cpf = "123", Cnpj = "" },
                Itens = new List<CriarPedido.CriarPedidoItemModel>()
            };
            var result = validator.Validate(command);
            result.IsValid.Should().BeFalse();
        }
    }
}
