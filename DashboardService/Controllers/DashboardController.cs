using Microsoft.AspNetCore.Mvc;

namespace DashboardService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    [HttpPost]
    public IActionResult Post([FromBody] object request)
    {
        return Ok(new { Service = "DashboardService", Status = "Dashboard data returned" });
    }

    [HttpGet("stats")]
    public IActionResult GetStats()
    {
        return Ok(new { Service = "DashboardService", Stats = new { TotalPolicies = 0, TotalAgents = 0, TotalRevenue = 0m } });
    }
}

[ApiController]
[Route("info")]
public class InfoController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Service = "DashboardService", Version = "1.0.0" });
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