using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using DashboardService.Api.Queries;
using DashboardService.Implementation.Queries;
using DashboardService.Implementation.Listeners;
using DashboardService.Implementation.DataAccess.Elastic;
using DashboardService.Implementation.Init;
using DashboardService.Implementation.Messaging.RabbitMq;

namespace DashboardService.Implementation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDashboardService(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
        return services;
    }

    public static IServiceCollection AddDashboardInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddElasticSearch(configuration.GetConnectionString("ElasticSearchConnection"));
        services.AddRabbitListeners(configuration.GetSection("RabbitMqOptions").Get<RabbitMqOptions>());
        services.AddInitialSalesData();
        return services;
    }
}