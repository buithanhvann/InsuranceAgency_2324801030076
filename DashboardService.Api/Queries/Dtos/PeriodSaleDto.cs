using System;

namespace DashboardService.Api.Queries.Dtos;

public class PeriodSaleDto
{
    public DateTime PeriodDate { get; set; }

    public string Period { get; set; } = default!;

    public SalesDto Sales { get; set; } = default!;
}