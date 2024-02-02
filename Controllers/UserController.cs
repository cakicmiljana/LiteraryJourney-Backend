using Microsoft.AspNetCore.Mvc;

namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "Hello from UserController!";
    }
}