using backend.model;

namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    private readonly IMongoClient _client;
    private readonly IMongoDatabase _db;
    public UserController(IMongoClient client)
    {
        _client = client;
        _db = _client.GetDatabase("Books");
    }

    [HttpPost("CreateUser")]
    public async Task<ActionResult> CreateUser([FromBody] User user)
    {
        return Ok("User created!");
    }

    [HttpGet("GetUser/{id}")]
    public async Task<ActionResult> GetUser(int id)
    {
        return Ok("Hello from UserController!");
    }

    [HttpPut("UpdateUser")]
    public async Task<ActionResult> UpdateUser([FromBody] User user)
    {
        return Ok("User updated!");
    }
}