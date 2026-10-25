using DashboardService.Api.Queries;
using DashboardService.Api.Queries.Dtos;
using MediatR;

namespace DashboardService.Implementation.Queries.Handlers;

public class GetTotalSalesHandler : IRequestHandler<GetTotalSalesQuery, GetTotalSalesResult>
{
    public Task<GetTotalSalesResult> Handle(GetTotalSalesQuery request, CancellationToken cancellationToken)
    {
        var result = new GetTotalSalesResult
        {
            Total = new SalesSummaryDto
            {
                PremiumAmount = 0,
                PoliciesCount = 0
            },
            PerProductTotal = new Dictionary<string, SalesSummaryDto>()
        };
        return Task.FromResult(result);
    }
}

public class GetAgentsSalesHandler : IRequestHandler<GetAgentsSalesQuery, GetAgentsSalesResult>
{
    public Task<GetAgentsSalesResult> Handle(GetAgentsSalesQuery request, CancellationToken cancellationToken)
    {
        var result = new GetAgentsSalesResult
        {
            PerAgentTotal = new Dictionary<string, SalesSummaryDto>()
        };
        return Task.FromResult(result);
    }
}

public class GetSalesTrendsHandler : IRequestHandler<GetSalesTrendsQuery, GetSalesTrendsResult>
{
    public Task<GetSalesTrendsResult> Handle(GetSalesTrendsQuery request, CancellationToken cancellationToken)
    {
        var result = new GetSalesTrendsResult
        {
            PeriodsSales = new List<PeriodSalesDto>()
        };
        return Task.FromResult(result);
    }
}