// Reseller.Test/Domain/ValueObjects/CnpjValueObjectTests.cs
using FluentAssertions;
using Reseller.Domain.Data.ValueObjects;
using Xunit;

namespace Reseller.Test.Domain.ValueObjects
{
    public class CnpjValueObjectTests
    {
        [Fact]
        public void Should_Create_CnpjValueObject_When_Valid()
        {
            var cnpj = new CnpjValueObject("12345678000195");
            cnpj.Value.Should().Be("12345678000195");
        }

        [Theory]
        [InlineData("")]
        [InlineData("123")]
        [InlineData("00000000000000")]
        [InlineData("1234567800019A")]
        public void Should_Throw_When_Invalid(string invalidCnpj)
        {
            Action act = () => new CnpjValueObject(invalidCnpj);
            act.Should().Throw<ArgumentException>();
        }
    }
}