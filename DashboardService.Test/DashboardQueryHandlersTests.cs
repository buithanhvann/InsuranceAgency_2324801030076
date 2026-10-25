using DashboardService.Api.Queries;
using DashboardService.Api.Queries.Dtos;
using DashboardService.Implementation.Queries.Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DashboardService.Test;

public class DashboardQueryHandlersTests
{
    private readonly IMediator _mediator;

    public DashboardQueryHandlersTests()
    {
        var services = new ServiceCollection();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetTotalSalesHandler).Assembly));
        var provider = services.BuildServiceProvider();
        _mediator = provider.GetRequiredService<IMediator>();
    }

    [Fact]
    public async Task GetTotalSalesHandler_ReturnsResult()
    {
        var query = new GetTotalSalesQuery
        {
            ProductCode = "TRI",
            SalesDateFrom = DateTime.UtcNow.AddDays(-30),
            SalesDateTo = DateTime.UtcNow
        };

        var result = await _mediator.Send(query);

        Assert.NotNull(result);
        Assert.NotNull(result.Total);
        Assert.NotNull(result.PerProductTotal);
    }

    [Fact]
    public async Task GetAgentsSalesHandler_ReturnsResult()
    {
        var query = new GetAgentsSalesQuery
        {
            SalesDateFrom = DateTime.UtcNow.AddDays(-30),
            SalesDateTo = DateTime.UtcNow
        };

        var result = await _mediator.Send(query);

        Assert.NotNull(result);
        Assert.NotNull(result.PerAgentTotal);
    }

    [Fact]
    public async Task GetSalesTrendsHandler_ReturnsResult()
    {
        var query = new GetSalesTrendsQuery
        {
            SalesDateFrom = DateTime.UtcNow.AddDays(-30),
            SalesDateTo = DateTime.UtcNow,
            Unit = TimeUnit.Day
        };

        var result = await _mediator.Send(query);

        Assert.NotNull(result);
        Assert.NotNull(result.PeriodsSales);
    }
}