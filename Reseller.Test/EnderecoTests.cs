// Reseller.Test/Domain/Entities/EnderecoTests.cs
using FluentAssertions;
using Reseller.Domain.Data.ValueObjects;
using Reseller.Domain.Entities;
using Xunit;

namespace Reseller.Test.Domain.Entities
{
    public class EnderecoTests
    {
        [Fact]
        public void Should_Create_Endereco_When_Valid()
        {
            var endereco = new Endereco(
                new LogradouroEnderecoValueObject("Rua Teste"),
                new NumeroEnderecoValueObject("123"),
                new ComplementoEnderecoValueObject(""),
                new CidadeEnderecoValueObject("Cidade"),
                new EstadoEnderecoValueObject("SP"),
                new CepValueObject("12345678")
            );

            endereco.Logradouro.Should().Be("Rua Teste");
            endereco.Numero.Should().Be("123");
        }

        [Fact]
        public void Should_Throw_When_Logradouro_Is_Empty()
        {
            Action act = () => new LogradouroEnderecoValueObject("");
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_Throw_When_Logradouro_Exceeds_MaxLength()
        {
            var value = new string('a', LogradouroEnderecoValueObject.MaxLength + 1);
            Action act = () => new LogradouroEnderecoValueObject(value);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Should_Throw_When_Numero_Is_Empty()
        {
            Action act = () => new NumeroEnderecoValueObject("");
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_Throw_When_Numero_Exceeds_MaxLength()
        {
            var value = new string('1', NumeroEnderecoValueObject.MaxLength + 1);
            Action act = () => new NumeroEnderecoValueObject(value);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Should_Throw_When_Complemento_Exceeds_MaxLength()
        {
            var value = new string('a', ComplementoEnderecoValueObject.MaxLength + 1);
            Action act = () => new ComplementoEnderecoValueObject(value);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Should_Throw_When_Cidade_Is_Empty()
        {
            Action act = () => new CidadeEnderecoValueObject("");
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_Throw_When_Cidade_Exceeds_MaxLength()
        {
            var value = new string('a', CidadeEnderecoValueObject.MaxLength + 1);
            Action act = () => new CidadeEnderecoValueObject(value);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Should_Throw_When_Estado_Is_Empty()
        {
            Action act = () => new EstadoEnderecoValueObject("");
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_Throw_When_Estado_Exceeds_MaxLength()
        {
            var value = new string('a', EstadoEnderecoValueObject.MaxLength + 1);
            Action act = () => new EstadoEnderecoValueObject(value);
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Should_Throw_When_Cep_Is_Invalid()
        {
            Action act = () => new CepValueObject("123");
            act.Should().Throw<ArgumentException>();
        }
    }
}