using Polly;
using Polly.Extensions.Http;

namespace OrderService.HttpClients.Policies;

public static class HttpPolicy
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
}