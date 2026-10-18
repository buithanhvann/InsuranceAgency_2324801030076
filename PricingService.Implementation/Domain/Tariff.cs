using System;
using System.Collections.Generic;

namespace PricingService.Domain;

public class Tariff
{
    public string Code { get; }

    public Tariff(string code)
    {
        Code = code;
    }

    public Price CalculatePrice(Calculation calculation)
    {
        return Code switch
        {
            "TRI" => CalculateTravelPrice(calculation),
            "HSI" => CalculateHousePrice(calculation),
            "FAI" => CalculateFarmPrice(calculation),
            "CAR" => CalculateCarPrice(calculation),
            _ => new Price { TotalPremium = 0, Covers = new Dictionary<string, CoverPrice>() }
        };
    }

    private Price CalculateTravelPrice(Calculation calculation)
    {
        var price = new Price();
        var adults = GetDecimal(calculation, "NUM_OF_ADULTS");
        var children = GetDecimal(calculation, "NUM_OF_CHILDREN");
        var destination = GetString(calculation, "DESTINATION");
        var isEur = destination == "EUR";
        var baseRate = isEur ? 26M : 34M;

        foreach (var cover in calculation.SelectedCovers)
        {
            decimal coverPrice = cover switch
            {
                "C1" => adults * baseRate,
                "C2" => (adults + children) * 26M,
                "C3" => (adults + children) * 10M,
                _ => 0M
            };

            // Apply markup for WORLD destination
            if (!isEur && destination == "WORLD")
                coverPrice *= 1.5M;

            price.Covers[cover] = new CoverPrice { Price = coverPrice };
            price.TotalPremium += coverPrice;
        }

        return price;
    }

    private Price CalculateHousePrice(Calculation calculation)
    {
        var price = new Price();
        var typ = GetString(calculation, "TYP");
        var area = GetDecimal(calculation, "AREA");
        var numOfClaim = GetDecimal(calculation, "NUM_OF_CLAIM");
        var flood = GetString(calculation, "FLOOD");

        foreach (var cover in calculation.SelectedCovers)
        {
            decimal coverPrice = cover switch
            {
                "C1" => typ == "APT" ? area * 1.25M : area * 1.50M,
                "C2" => typ == "APT" ? area * 0.25M : area * 0.45M,
                "C3" => 30M,
                "C4" => 50M,
                _ => 0M
            };

            // Apply markups
            if (flood == "YES") coverPrice *= 1.50M;
            if (numOfClaim > 1) coverPrice *= 1.25M;

            price.Covers[cover] = new CoverPrice { Price = coverPrice };
            price.TotalPremium += coverPrice;
        }

        return price;
    }

    private Price CalculateFarmPrice(Calculation calculation)
    {
        var price = new Price();
        var flood = GetString(calculation, "FLOOD");
        var numOfClaim = GetDecimal(calculation, "NUM_OF_CLAIM");

        foreach (var cover in calculation.SelectedCovers)
        {
            decimal coverPrice = cover switch
            {
                "C1" => 10M,
                "C2" => 20M,
                "C3" => 30M,
                "C4" => 40M,
                _ => 0M
            };

            if (flood == "YES") coverPrice *= 1.50M;
            if (numOfClaim > 2) coverPrice *= 2.00M;

            price.Covers[cover] = new CoverPrice { Price = coverPrice };
            price.TotalPremium += coverPrice;
        }

        return price;
    }

    private Price CalculateCarPrice(Calculation calculation)
    {
        var price = new Price();
        var numOfClaim = GetDecimal(calculation, "NUM_OF_CLAIM");

        foreach (var cover in calculation.SelectedCovers)
        {
            decimal coverPrice = cover switch
            {
                "C1" => 100M,
                _ => 0M
            };

            if (numOfClaim > 2) coverPrice *= 1.50M;

            price.Covers[cover] = new CoverPrice { Price = coverPrice };
            price.TotalPremium += coverPrice;
        }

        return price;
    }

    private decimal GetDecimal(Calculation calculation, string key)
    {
        if (calculation.Subject.TryGetValue(key, out var value))
        {
            return value switch
            {
                decimal d => d,
                string s when decimal.TryParse(s, out var parsed) => parsed,
                int i => i,
                _ => 0M
            };
        }
        return 0M;
    }

    private string GetString(Calculation calculation, string key)
    {
        if (calculation.Subject.TryGetValue(key, out var value))
        {
            return value?.ToString() ?? "";
        }
        return "";
    }
}