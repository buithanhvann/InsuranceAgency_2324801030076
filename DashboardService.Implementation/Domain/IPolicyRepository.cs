using DashboardService.Api.Queries;

namespace DashboardService.Implementation.Domain;

public interface IPolicyRepository
{
    void Save(PolicyDocument policy);

    PolicyDocument FindByNumber(string policyNumber);

    AgentSalesQueryResult GetAgentSales(AgentSalesQuery query);

    TotalSalesQueryResult GetTotalSales(TotalSalesQuery query);

    SalesTrendsResult GetSalesTrend(SalesTrendsQuery query);
}