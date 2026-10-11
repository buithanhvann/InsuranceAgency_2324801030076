using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace PricingService.Implementation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPricingService(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
        return services;
    }
}

public record PingRequest(string Message) : IRequest<string>;

public class PingHandler : IRequestHandler<PingRequest, string>
{
    public Task<string> Handle(PingRequest request, CancellationToken cancellationToken)
        => Task.FromResult($"PricingService received: {request.Message}");
}
