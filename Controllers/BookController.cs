using backend.model;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IMongoClient _client;
    private IMongoDatabase _db;
    
    
    public BookController(IMongoClient client)
    {
        _client = client;
        _db = _client.GetDatabase("Books");
    }

    [HttpPost("CreateBook")]
    public async Task<ActionResult> CreateBook()
    {
        List<string> databases = _client.ListDatabaseNames().ToList();
        var col = await _db.ListCollectionNamesAsync().Result.ToListAsync();
        return Ok(col);
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