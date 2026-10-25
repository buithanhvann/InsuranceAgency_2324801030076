using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PricingService.Api.Commands;
using PricingService.Controllers;
using Xunit;

namespace PricingService.Test.Controllers;

public class PricingControllerShould
{
    private static IValidator<CalculatePriceCommand> CreateValidValidator()
    {
        var validator = new Mock<IValidator<CalculatePriceCommand>>();
        validator
            .Setup(x => x.ValidateAsync(It.IsAny<CalculatePriceCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        return validator.Object;
    }

    [Fact]
    public async Task CallMediatorWithCorrectParameters()
    {
        var mediator = new Mock<IMediator>();
        var command = new CalculatePriceCommand();
        var controller = new PricingController(mediator.Object, CreateValidValidator());

        await controller.Post(command);

        mediator.Verify(x => x.Send(command, default), Times.Once());
    }

    [Fact]
    public async Task ReturnOkResultWithCalculatePriceResult()
    {
        var mediator = new Mock<IMediator>();
        var command = new CalculatePriceCommand();
        var calculatePriceResult = new CalculatePriceResult { TotalPrice = 12 };
        mediator.Setup(x => x.Send(command, default)).ReturnsAsync(calculatePriceResult);
        var controller = new PricingController(mediator.Object, CreateValidValidator());

        var result = await controller.Post(command);

        var okResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
        Assert.Equal(calculatePriceResult, okResult.Value);
    }
}
