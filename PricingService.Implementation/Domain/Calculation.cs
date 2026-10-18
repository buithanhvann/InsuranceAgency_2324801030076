using System;
using System.Collections.Generic;
using System.Linq;

namespace PricingService.Domain;

public class Calculation
{
    public string ProductCode { get; }
    public DateTimeOffset PolicyFrom { get; }
    public DateTimeOffset PolicyTo { get; }
    public List<string> SelectedCovers { get; }
    public Dictionary<string, object> Subject { get; }

    public Calculation(string productCode, DateTimeOffset policyFrom, DateTimeOffset policyTo, List<string> selectedCovers, Dictionary<string, object> subject)
    {
        ProductCode = productCode;
        PolicyFrom = policyFrom;
        PolicyTo = policyTo;
        SelectedCovers = selectedCovers;
        Subject = subject;
    }

    public T GetValue<T>(string key)
    {
        if (Subject.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }
        return default!;
    }
}