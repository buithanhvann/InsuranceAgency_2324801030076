using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PolicyService.Api.Commands;

namespace PolicyService.Implementation;

public static class ServiceCollectionExtensions
{
    // Task #2.1: Dang ky MediatR trong DI container (tuong duong services.AddMediatR() o Startup).
    public static IServiceCollection AddPolicyService(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
        return services;
    }
}

// Vi du minh hoa CQRS: handler xu ly lenh tao offer.
public record CalculateOfferRequest(CreateOfferCommand Command) : IRequest<CreateOfferResult>;

public class CalculateOfferHandler : IRequestHandler<CalculateOfferRequest, CreateOfferResult>
{
    public Task<CreateOfferResult> Handle(CalculateOfferRequest request, CancellationToken cancellationToken)
    {
        // Logic tinh gia that se goi PricingService qua REST; day la stub de build thanh cong.
        return Task.FromResult(new CreateOfferResult
        {
            OfferNumber = Guid.NewGuid().ToString("N"),
            TotalPrice = 0m
        });
    }
}
