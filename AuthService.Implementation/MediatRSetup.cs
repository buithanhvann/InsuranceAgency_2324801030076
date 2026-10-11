using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Implementation;

public static class ServiceCollectionExtensions
{
    // Task #2.1: Dang ky MediatR trong DI container.
    public static IServiceCollection AddAuthService(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
        return services;
    }
}

public record PingRequest(string Message) : IRequest<string>;

public class PingHandler : IRequestHandler<PingRequest, string>
{
    public Task<string> Handle(PingRequest request, CancellationToken cancellationToken)
        => Task.FromResult($"AuthService received: {request.Message}");
}
