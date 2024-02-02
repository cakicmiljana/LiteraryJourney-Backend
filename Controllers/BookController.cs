using Microsoft.AspNetCore.Mvc;

namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "Hello from BookController!";
    }
}