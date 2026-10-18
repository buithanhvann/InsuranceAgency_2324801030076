using System.Collections.Generic;
using System.Threading.Tasks;
using PricingService.Domain;

namespace PricingService.Commands;

public interface IDataStore
{
    Dictionary<string, Tariff> Tariffs { get; set; }
    Task<Tariff> GetTariffAsync(string productCode);
}