using DashboardService.Api.Queries.Dtos;
using MediatR;

namespace DashboardService.Api.Queries;

public enum TimeUnit
{
    Day,
    Week,
    Month,
    Year
}

public class GetTotalSalesQuery : IRequest<GetTotalSalesResult>
{
    public string? ProductCode { get; set; }
    public DateTime SalesDateFrom { get; set; }
    public DateTime SalesDateTo { get; set; }
}

public class GetTotalSalesResult
{
    public SalesSummaryDto Total { get; set; } = new();
    public Dictionary<string, SalesSummaryDto> PerProductTotal { get; set; } = new();
}

public class GetAgentsSalesQuery : IRequest<GetAgentsSalesResult>
{
    public DateTime SalesDateFrom { get; set; }
    public DateTime SalesDateTo { get; set; }
}

public class GetAgentsSalesResult
{
    public Dictionary<string, SalesSummaryDto> PerAgentTotal { get; set; } = new();
}

public class GetSalesTrendsQuery : IRequest<GetSalesTrendsResult>
{
    public DateTime SalesDateFrom { get; set; }
    public DateTime SalesDateTo { get; set; }
    public TimeUnit Unit { get; set; }
}

public class GetSalesTrendsResult
{
    public List<PeriodSalesDto> PeriodsSales { get; set; } = new();
}
