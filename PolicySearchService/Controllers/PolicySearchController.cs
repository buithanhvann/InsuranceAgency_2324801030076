using Microsoft.AspNetCore.Mvc;

namespace PolicySearchService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PolicySearchController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Service = "PolicySearchService", Status = "Running" });
    }
}

[ApiController]
[Route("info")]
public class InfoController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Service = "PolicySearchService", Version = "1.0.0" });
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