using MediatR;
using Microsoft.Extensions.DependencyInjection;
using DashboardService.Api.Queries;
using DashboardService.Implementation.Queries.Handlers;

namespace DashboardService.Implementation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDashboardService(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
        return services;
    }
}