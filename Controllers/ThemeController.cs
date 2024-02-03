using backend.model;
using Microsoft.AspNetCore.Mvc;

namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class ThemeController : ControllerBase
{
    ThemeController()
    {
        
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