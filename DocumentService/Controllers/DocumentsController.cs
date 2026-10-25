using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Service = "DocumentService", Status = "Running" });
    }

    [HttpPost]
    public IActionResult Post([FromBody] object document)
    {
        return Ok(new { Service = "DocumentService", Status = "Stored" });
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        return Ok(new { Service = "DocumentService", DocumentId = id, Status = "Found" });
    }
}

[ApiController]
[Route("info")]
public class InfoController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Service = "DocumentService", Version = "1.0.0" });
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