using Play.Inventory.Service.Clients;
using Polly;
using Polly.Extensions.Http;
namespace Play.Inventory.Service.Clients;
public static class ServiceExtensions
{
    public static IServiceCollection AddCatalogClient(this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration.GetValue<string>("CatalogEndpoint:BaseUrl")!;

        services.AddHttpClient<CatalogClient>(client =>
        {
            client.BaseAddress = new Uri(url);
        })
        .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutException>().CircuitBreakerAsync(
            3,
            TimeSpan.FromSeconds(10),
            onBreak: (outcome, timespan) =>
            {
                var serviceProvider = services.BuildServiceProvider();
                serviceProvider.GetService<ILogger<CatalogClient>>()?
                    .LogWarning($"Closing Circuit for {timespan.TotalSeconds} seconds..");
            },
            onReset: () =>
            {
                var serviceProvider = services.BuildServiceProvider();
                serviceProvider.GetService<ILogger<CatalogClient>>()?
                    .LogWarning($"Closing Circuit ");
            }
        ));
        return services;
    }
}