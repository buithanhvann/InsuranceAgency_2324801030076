using PricingService.Domain;

namespace PricingService.Test.Domain;

internal static class TariffFactory
{
    internal static Tariff Travel() => new("TRI");
    internal static Tariff House() => new("HSI");
    internal static Tariff Farm() => new("FAI");
    internal static Tariff Car() => new("CAR");
}