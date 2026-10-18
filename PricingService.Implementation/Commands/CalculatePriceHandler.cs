using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using PricingService.Api.Commands;
using PricingService.Api.Commands.Dto;
using PricingService.Commands;
using PricingService.Domain;

namespace PricingService.Commands;

public class CalculatePriceHandler : IRequestHandler<CalculatePriceCommand, CalculatePriceResult>
{
    private readonly IDataStore dataStore;
    private readonly CalculatePriceCommandValidator validator = new();

    public CalculatePriceHandler(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<CalculatePriceResult> Handle(CalculatePriceCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var tariff = await dataStore.GetTariffAsync(request.ProductCode);
        if (tariff == null)
        {
            throw new ValidationException($"Tariff not found for product code: {request.ProductCode}");
        }

        var subject = new Dictionary<string, object>();
        foreach (var answer in request.Answers)
        {
            subject[answer.QuestionCode] = answer.GetAnswer();
        }

        var calculation = new Calculation(
            request.ProductCode,
            request.PolicyFrom,
            request.PolicyTo,
            request.SelectedCovers,
            subject
        );

        var price = tariff.CalculatePrice(calculation);

        var coverPrices = new Dictionary<string, decimal>();
        foreach (var kvp in price.Covers)
        {
            coverPrices[kvp.Key] = kvp.Value.Price;
        }

        return new CalculatePriceResult
        {
            TotalPrice = price.TotalPremium,
            CoverPrices = coverPrices
        };
    }
}

public class InMemoryDataStore : IDataStore
{
    public Dictionary<string, Tariff> Tariffs { get; set; } = new();

    public Task<Tariff> GetTariffAsync(string productCode)
    {
        Tariffs.TryGetValue(productCode, out var tariff);
        return Task.FromResult(tariff)!;
    }
}