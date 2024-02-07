using backend.model;

namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _db;
    public ReviewController(IMongoClient client)
    {
        _client = client;
        _db = _client.GetDatabase("Books");
    }

    [HttpPost("CreateReview")]
    public async Task<ActionResult> CreateReview([FromBody] Review review)
    {
        return Ok("Review created!");
    }

    [HttpGet("GetReview/{id}")]
    public async Task<ActionResult> GetReview(int id)
    {
        return Ok("Hello from ReviewController!");
    }

    [HttpPut("UpdateReview")]
    public async Task<ActionResult> UpdateReview([FromBody] Review review)
    {
        return Ok("Review updated!");
    }
}