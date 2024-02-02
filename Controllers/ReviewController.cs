using Microsoft.AspNetCore.Mvc;

namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "Hello from ReviewController!";
    }
}