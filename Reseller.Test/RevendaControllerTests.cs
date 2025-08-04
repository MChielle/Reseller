// Reseller.Test/Controllers/RevendaControllerTests.cs
using Xunit;
using Moq;
using Reseller.Domain.Interfaces.Services;
using Reseller.Domain.Models.RevendaModels.Get;
using Reseller.Domain.Models.RevendaModels.Input;
using Reseller.Domain.Models.RevendaModels.GetByCnpj;
using System.Threading.Tasks;
using Reseller.API.Controllers;
using Microsoft.Extensions.Logging;
using Reseller.Domain.Models.RevendaModels.Create;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace Reseller.Test.Controllers
{
    public class RevendaControllerTests
    {
        [Fact]
        public async Task CreateRevendaAsync_ShouldReturnCreated_WhenModelIsValid()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<RevendaController>>();
            var mockService = new Mock<IRevendaService>();
            mockService.Setup(s => s.CreateRevendaAsync(It.IsAny<RevendaCreateModelRequest>()))
                .ReturnsAsync(true);

            var controller = new RevendaController(mockLogger.Object, mockService.Object);
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
            var result = await controller.CreateRevendaAsync(request);

            // Assert
            result.Should().BeOfType<CreatedResult>();
        }

        [Fact]
        public async Task CreateRevendaAsync_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<RevendaController>>();
            var mockService = new Mock<IRevendaService>();
            var controller = new RevendaController(mockLogger.Object, mockService.Object);

            var request = new RevendaCreateModelRequest();

            controller.ModelState.AddModelError("Cnpj", "CNPJ é obrigatório.");

            // Act
            var result = await controller.CreateRevendaAsync(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetRevendaByCnpjAsync_ShouldReturnOk_WhenRevendaExists()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<RevendaController>>();
            var mockService = new Mock<IRevendaService>();
            var response = new RevendaGetByIdModelResponse("12345678000195", "Empresa Teste", "Fantasia Teste", "teste@empresa.com");
            mockService.Setup(s => s.GetRevendaByIdAsync(It.IsAny<RevendaGetByIdModelRequest>()))
                .ReturnsAsync(response);

            var controller = new RevendaController(mockLogger.Object, mockService.Object);
            var request = new RevendaGetByIdModelRequest { Id = Guid.NewGuid() };

            // Act
            var result = await controller.GetRevendaByIdAsync(request);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task GetRevendaByCnpjAsync_ShouldReturnBadRequest_WhenServiceThrows()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<RevendaController>>();
            var mockService = new Mock<IRevendaService>();
            mockService.Setup(s => s.GetRevendaByIdAsync(It.IsAny<RevendaGetByIdModelRequest>()))
                .ThrowsAsync(new System.Exception("Revenda não encontrada."));

            var controller = new RevendaController(mockLogger.Object, mockService.Object);
            var request = new RevendaGetByIdModelRequest { Id = Guid.NewGuid() };

            // Act
            var result = await controller.GetRevendaByIdAsync(request);

            // Assert
            var badRequest = result as BadRequestObjectResult;
            badRequest.Should().NotBeNull();
            badRequest.Value.Should().Be("Revenda não encontrada.");
        }
    }
}