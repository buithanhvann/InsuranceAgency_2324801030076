using System;
using System.Collections.Generic;

namespace PricingService.Domain;

public class Price
{
    public decimal TotalPremium { get; set; }
    public Dictionary<string, CoverPrice> Covers { get; set; } = new();
}

public class CoverPrice
{
    public decimal Price { get; set; }
}