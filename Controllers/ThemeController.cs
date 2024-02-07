using backend.model;

namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class ThemeController : ControllerBase
{
    
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _db;
    public ThemeController(IMongoClient client)
    {
        _client = client;
        _db = _client.GetDatabase("Books");
    }

    [HttpPost("CreateTheme")]
    public async Task<ActionResult> CreateTheme([FromBody] Theme theme)
    {
        return Ok("Theme created!");
    }

    [HttpGet("GetTheme/{id}")]
    public async Task<ActionResult> GetTheme(int id)
    {
        return Ok("Hello from ThemeController!");
    }

    [HttpPut("UpdateTheme")]
    public async Task<ActionResult> UpdateTheme([FromBody] Theme theme)
    {
        return Ok("Theme updated!");
    }
}