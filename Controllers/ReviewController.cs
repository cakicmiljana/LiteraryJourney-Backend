using backend.model;
using Microsoft.AspNetCore.Mvc;

namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    ReviewController()
    {
        
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