using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] object credentials)
    {
        return Ok(new { Service = "AuthService", Token = "sample-jwt-token", Status = "Authenticated" });
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] object user)
    {
        return Ok(new { Service = "AuthService", Status = "Registered" });
    }
}

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