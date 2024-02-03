using backend.model;
using Microsoft.AspNetCore.Mvc;

namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    
    public BookController()
    {

    }

    [HttpPost("CreateBook")]
    public async Task<ActionResult> CreateBook([FromBody] Book book)
    {
        return Ok("Book created!");
    }

    [HttpGet("GetBook/{id}")]
    public async Task<ActionResult> GetBook(int id)
    {
        return Ok("Hello from UserController!");
    }

    [HttpPut("UpdateBook")]
    public async Task<ActionResult> UpdateBook([FromBody] Book book)
    {
        return Ok("User updated!");
    }
}