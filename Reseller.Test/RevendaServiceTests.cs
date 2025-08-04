using FluentAssertions;
using Moq;
using Reseller.Domain.Data.ValueObjects;
using Reseller.Domain.Entities;
using Reseller.Domain.Interfaces.Repositories;
using Reseller.Domain.Models.RevendaModels.Create;
using Reseller.Domain.Models.RevendaModels.Get;
using Reseller.Domain.Models.RevendaModels.Input;
using Reseller.Service.Services;


namespace Reseller.Test.Services
{
    public class RevendaServiceTests
    {
        [Fact]
        public async Task CreateRevendaAsync_ShouldReturnTrue_WhenRevendaIsCreated()
        {
            // Arrange
            var mockRepo = new Mock<IRevendaRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Revenda)null);
            mockRepo.Setup(r => r.CreateAsync(It.IsAny<Revenda>()))
                .ReturnsAsync(true);

            var service = new RevendaService(mockRepo.Object);

            var request = new RevendaCreateModelRequest
            {
                Cnpj = "12345678000195",
                RazaoSocial = "Empresa Teste",
                NomeFantasia = "Fantasia Teste",
                Email = "teste@empresa.com",
                Telefones = new() { "11999999999" },
                Contatos = new() { new RevendaCreateContatoModel { Nome = "Contato", Principal = true } },
                Enderecos = new() { new RevendaCreateEnderecoModel { Logradouro = "Rua Teste", Numero = "123", Complemento = "", Cidade = "Cidade", Estado = "SP", Cep = "12345678" } }
            };

            // Act
            var result = await service.CreateRevendaAsync(request);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetRevendaByCnpjAsync_ShouldReturnModel_WhenRevendaExists()
        {
            // Arrange
            var cnpj = "12345678000195";
            var revendaEntity = new Revenda(
                new CnpjValueObject(cnpj),
                new RazaoSocialRevendaValueObject("Empresa Teste"),
                new NomeFantasiaRevendaValueObject("Fantasia Teste"),
                new EmailValueObject("teste@empresa.com"),
                new List<Telefone>(),
                new List<Contato>(),
                new List<Endereco>()
            );

            var mockRepo = new Mock<IRevendaRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(revendaEntity);

            var service = new RevendaService(mockRepo.Object);

            var request = new RevendaGetByIdModelRequest { Id = Guid.NewGuid()};

            // Act
            var result = await service.GetRevendaByIdAsync(request);

            // Assert
            result.Should().NotBeNull();
            result.Cnpj.Should().Be(cnpj);
            result.RazaoSocial.Should().Be("Empresa Teste");
            result.NomeFantasia.Should().Be("Fantasia Teste");
            result.Email.Should().Be("teste@empresa.com");
        }

        [Fact]
        public async Task GetRevendaByIdAsync_ShouldReturnNoContent_WhenRevendaDoesNotExist()
        {
            // Arrange
            var cnpj = "12345678000195";
            var mockRepo = new Mock<IRevendaRepository>();
            mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Revenda)null);

            var service = new RevendaService(mockRepo.Object);

            var request = new RevendaGetByIdModelRequest { Id = Guid.NewGuid() };

            // Act
            var result = await service.GetRevendaByIdAsync(request);

            // Assert
            result.Should().Be(null);
        }
    }
}
