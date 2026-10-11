namespace DashboardService.Api.Queries.Dtos;

public class SalesSummaryDto
{
    public decimal PremiumAmount { get; set; }
    public long PoliciesCount { get; set; }
}

public class PeriodSalesDto
{
    public string Period { get; set; } = string.Empty;
    public SalesSummaryDto Sales { get; set; } = new();
}
