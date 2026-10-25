using Microsoft.AspNetCore.Mvc;

namespace PaymentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Service = "PaymentService", Status = "Running" });
    }

    [HttpPost]
    public IActionResult Post([FromBody] object payment)
    {
        return Ok(new { Service = "PaymentService", Status = "Processed" });
    }
}

[ApiController]
[Route("info")]
public class InfoController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Service = "PaymentService", Version = "1.0.0" });
    }
}

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow });
    }
}