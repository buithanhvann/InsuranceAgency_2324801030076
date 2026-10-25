using Microsoft.AspNetCore.Mvc;

namespace PolicyService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PoliciesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Service = "PolicyService", Status = "Running" });
    }

    [HttpGet("{number}")]
    public IActionResult Get(string number)
    {
        return Ok(new { Service = "PolicyService", PolicyNumber = number, Status = "Found" });
    }

    [HttpPost]
    public IActionResult Post([FromBody] object policy)
    {
        return Ok(new { Service = "PolicyService", Status = "Created" });
    }
}

[ApiController]
[Route("api/Offer")]
public class OfferController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] object offer)
    {
        return Ok(new { Service = "PolicyService", Status = "OfferCreated" });
    }
}

[ApiController]
[Route("info")]
public class InfoController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Service = "PolicyService", Version = "1.0.0" });
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