using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PricingService.Api.Commands;
using PricingService.Controllers;
using Xunit;

namespace PricingService.Test.Controllers;

public class PricingControllerShould
{
    [Fact]
    public async Task CallMediatorWithCorrectParameters()
    {
        var mediator = new Mock<IMediator>();
        var command = new CalculatePriceCommand();
        var controller = new PricingController(mediator.Object);

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
        var controller = new PricingController(mediator.Object);

        var result = await controller.Post(command);

        var okResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
        Assert.Equal(calculatePriceResult, okResult.Value);
    }
}