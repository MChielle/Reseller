// Reseller.Test/Domain/Models/RevendaCreateModelRequestTests.cs
using FluentAssertions;
using Reseller.Domain.Models.RevendaModels.Create;
using Reseller.Domain.Models.RevendaModels.Input;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Reseller.Test.Domain.Models
{
    public class RevendaCreateModelRequestTests
    {
        [Fact]
        public void Should_Return_ValidationError_When_Cnpj_Is_Invalid()
        {
            var model = new RevendaCreateModelRequest
            {
                Cnpj = "123",
                RazaoSocial = "Empresa",
                NomeFantasia = "Fantasia",
                Email = "email@email.com",
                Telefones = new() { "11999999999" },
                Contatos = new() { new RevendaCreateContatoModel { Nome = "Contato", Principal = true } },
                Enderecos = new() { new RevendaCreateEnderecoModel { Logradouro = "Rua", Numero = "1", Complemento = "", Cidade = "Cidade", Estado = "SP", Cep = "12345678" } }
            };

            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, results, true);

            results.Should().Contain(r => r.ErrorMessage.Contains("CNPJ inválido"));
        }
    }
}