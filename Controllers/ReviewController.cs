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
        review.Id = ObjectId.GenerateNewId();
        await _db.GetCollection<Review>("ReviewCollection").InsertOneAsync(review);
        return Ok("Review created!" + review.Id);
    }

    [HttpGet("GetReviewById/{id}")]
    public async Task<ActionResult> GetReview(string id)
    {
        var filter = Builders<Review>.Filter.Eq(b => b.Id, new ObjectId(id));
        var review = await _db.GetCollection<Review>("ReviewCollection").Find(filter).FirstOrDefaultAsync();
        return Ok(review);
    }

    [HttpPut("UpdateReview/{id}/{rating}/{comment}")]
    public async Task<ActionResult> UpdateReview(string id, int rating, string comment)
    {
        var filter = Builders<Review>.Filter.Eq(b => b.Id, new ObjectId(id));
        var review = await _db.GetCollection<Review>("ReviewCollection").Find(filter).FirstOrDefaultAsync();
        if (rating < 1 || rating > 5)
        {
            return BadRequest("Rating must be between 1 and 5");
        }
        review.Rating = rating;
        review.Comment = comment;
        await _db.GetCollection<Review>("ReviewCollection").ReplaceOneAsync(filter, review);
        return Ok("Review updated!");
    }

    [HttpDelete("DeleteReview/{id}")]
    public async Task<ActionResult> DeleteReview(string id)
    {
        var filter = Builders<Review>.Filter.Eq(b => b.Id, new ObjectId(id));
        await _db.GetCollection<Review>("ReviewCollection").DeleteOneAsync(filter);
        return Ok("Review deleted!");
    }
}