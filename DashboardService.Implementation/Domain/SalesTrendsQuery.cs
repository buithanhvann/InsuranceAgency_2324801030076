using System;
using System.Collections.Generic;
using Dtos = DashboardService.Api.Queries.Dtos;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Aggregations;

namespace DashboardService.Implementation.Domain;

public class SalesTrendsQuery
{
    public SalesTrendsQuery(string filterByProductCode, DateTime filterBySalesDateStart, DateTime filterBySalesDateEnd,
        TimeAggregationUnit aggregationUnit)
    {
        FilterByProductCode = filterByProductCode;
        FilterBySalesDateStart = filterBySalesDateStart;
        FilterBySalesDateEnd = filterBySalesDateEnd;
        AggregationUnit = aggregationUnit;
    }

    public string FilterByProductCode { get; }
    public DateTime FilterBySalesDateStart { get; }
    public DateTime FilterBySalesDateEnd { get; }
    public TimeAggregationUnit AggregationUnit { get; }
}

public enum TimeAggregationUnit
{
    Day,
    Week,
    Month,
    Year
}

public static class TimeAggregationUnitExtensions
{
    public static CalendarInterval ToCalendarInterval(this TimeAggregationUnit unit) => unit switch
    {
        TimeAggregationUnit.Day => CalendarInterval.Day,
        TimeAggregationUnit.Week => CalendarInterval.Week,
        TimeAggregationUnit.Month => CalendarInterval.Month,
        TimeAggregationUnit.Year => CalendarInterval.Year,
        _ => throw new ArgumentException($"Invalid value of unit {unit}")
    };

    public static TimeAggregationUnit ToTimeAggregationUnit(this Dtos.TimeUnit unit) => unit switch
    {
        Dtos.TimeUnit.Day => TimeAggregationUnit.Day,
        Dtos.TimeUnit.Week => TimeAggregationUnit.Week,
        Dtos.TimeUnit.Month => TimeAggregationUnit.Month,
        Dtos.TimeUnit.Year => TimeAggregationUnit.Year,
        _ => throw new ArgumentException($"Invalid value of unit {unit}")
    };

}

public class SalesTrendsResult
{
    public SalesTrendsResult()
    {
        PeriodSales = new List<PeriodSales>();
    }

    public IList<PeriodSales> PeriodSales { get; }

    public void PeriodResult(PeriodSales periodSales)
    {
        PeriodSales.Add(periodSales);
    }
}

public class PeriodSales
{
    public PeriodSales(DateTime periodDate, string period, SalesResult sales)
    {
        PeriodDate = periodDate;
        Period = period;
        Sales = sales;
    }

    public DateTime PeriodDate { get; }
    public string Period { get; }
    public SalesResult Sales { get; }
}