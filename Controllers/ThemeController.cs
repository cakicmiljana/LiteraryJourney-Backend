using Microsoft.AspNetCore.Mvc;

namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class ThemeController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "Hello from ThemeController!";
    }
}