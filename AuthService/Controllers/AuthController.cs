using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("info")]
public class InfoController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Service = "AuthService", Version = "1.0.0" });
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