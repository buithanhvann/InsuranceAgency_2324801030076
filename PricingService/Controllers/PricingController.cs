using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PricingService.Api.Commands;

namespace PricingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PricingController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IValidator<CalculatePriceCommand> validator;

    public PricingController(IMediator mediator, IValidator<CalculatePriceCommand> validator)
    {
        this.mediator = mediator;
        this.validator = validator;
    }

    [HttpPost]
    public async Task<ActionResult<CalculatePriceResult>> Post([FromBody] CalculatePriceCommand command)
    {
        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { Errors = validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) });
        }

        var result = await mediator.Send(command);
        return Ok(result);
    }
}