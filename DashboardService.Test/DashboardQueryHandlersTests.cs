using DashboardService.Api.Queries;
using DashboardService.Api.Queries.Dtos;
using DashboardService.Implementation.Domain;
using DashboardService.Implementation.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace DashboardService.Test;

public class DashboardQueryHandlersTests
{
    private readonly IMediator _mediator;

    public DashboardQueryHandlersTests()
    {
        var services = new ServiceCollection();
        
        // Register a mock IPolicyRepository for testing
        var mockPolicyRepository = new Mock<IPolicyRepository>();
        
        // Setup mock for GetTotalSales
        mockPolicyRepository.Setup(x => x.GetTotalSales(It.IsAny<TotalSalesQuery>()))
            .Returns(new TotalSalesQueryResult());
        
        // Setup mock for GetAgentSales
        mockPolicyRepository.Setup(x => x.GetAgentSales(It.IsAny<AgentSalesQuery>()))
            .Returns(new AgentSalesQueryResult());
        
        // Setup mock for GetSalesTrend
        mockPolicyRepository.Setup(x => x.GetSalesTrend(It.IsAny<SalesTrendsQuery>()))
            .Returns(new SalesTrendsResult());
        
        services.AddSingleton(mockPolicyRepository.Object);
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
            AgentLogin = "test-agent",
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
            ProductCode = "TRI",
            SalesDateFrom = DateTime.UtcNow.AddDays(-30),
            SalesDateTo = DateTime.UtcNow,
            Unit = TimeUnit.Day
        };

        var result = await _mediator.Send(query);

        Assert.NotNull(result);
        Assert.NotNull(result.PeriodsSales);
    }
}