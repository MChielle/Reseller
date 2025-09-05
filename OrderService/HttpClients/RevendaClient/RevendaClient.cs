using OrderService.HttpClients.RevendaClient.Models;
using Shared;
using System.Net;
using System.Text.Json;
using System.Web;

namespace OrderService.HttpClients.Revenda
{
    public class RevendaClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerConfiguration;

        public RevendaClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonSerializerConfiguration = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<Result<GetRevendaByIdResponseModel>> GetRevendaByIdAsync(Guid Id)
        {
            if (Id == default)
            {
                return Result.Failure<GetRevendaByIdResponseModel>(new Error("GetRevendaByIdAsync", "Id Revenda é parâmetro obrigatório"));
            }

            var url = "/revenda";

            var query = HttpUtility.ParseQueryString(string.Empty);
            query["Id"] = Id.ToString();

            if (query.HasKeys()) url = string.Concat(url, "?", query.ToString());

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode == HttpStatusCode.NotFound) return null;
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GetRevendaByIdResponseModel>(content, _jsonSerializerConfiguration);
        }
    }
}