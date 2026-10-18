using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PricingService.Commands;
using PricingService.Domain;

namespace PricingService.Implementation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPricingService(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
        services.AddSingleton<IDataStore>(sp =>
        {
            var store = new InMemoryDataStore();
            store.Tariffs["TRI"] = CreateTravelTariff();
            store.Tariffs["HSI"] = CreateHouseTariff();
            store.Tariffs["FAI"] = CreateFarmTariff();
            store.Tariffs["CAR"] = CreateCarTariff();
            return store;
        });
        return services;
    }

    private static Tariff CreateTravelTariff() => new("TRI");
    private static Tariff CreateHouseTariff() => new("HSI");
    private static Tariff CreateFarmTariff() => new("FAI");
    private static Tariff CreateCarTariff() => new("CAR");

    public record PingRequest(string Message) : IRequest<string>;

    public class PingHandler : IRequestHandler<PingRequest, string>
    {
        public Task<string> Handle(PingRequest request, CancellationToken cancellationToken)
            => Task.FromResult($"PricingService received: {request.Message}");
    }
}