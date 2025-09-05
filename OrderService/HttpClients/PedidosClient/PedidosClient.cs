using OrderService.HttpClients.PedidosClient.Models;
using Shared;
using System.Text.Json;

namespace OrderService.HttpClients.PedidosClient
{
    public class PedidosClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerConfiguration;

        public PedidosClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonSerializerConfiguration = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<Result<Guid>> EnviarPedidosAsync(EnviarPedidosRequestModel pedidos)
        {
            //Mock operação de envio dos pedidos.
            //Em uma requisição real, o polly esta configurado para tentar novamente em caso de falha.
            var sucesso = new Random();
            if (sucesso.Next(0, 100) >= 90)
            {
                Task.Delay(3000).Wait();
                return Result.Failure<Guid>(new Error("EnviarPedidos.Failure", "Falha no envio dos pedidos."));
            }
            Task.Delay(1000).Wait();
            return Guid.NewGuid();
        }
    }
}