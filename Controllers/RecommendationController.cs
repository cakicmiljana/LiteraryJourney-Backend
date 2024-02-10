using backend.dto;
using backend.model;
using backend.services;
namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class RecommendationController : ControllerBase
{

    private readonly IMongoClient _client;
    private readonly IMongoDatabase _db;
    private readonly StatisticsService _statisticsServices;

    public RecommendationController(IMongoClient client)
    {
        _client = client;
        _db = _client.GetDatabase("Books");
        _statisticsServices = new StatisticsService(_client);
    }

    // [HttpGet("GetRecommendationByLanguage/{userId}")]

    // [HttpGet("GetRecommendationByAuthor/{userId}")]
    // [HttpGet("GetRecommendationByGenre/{userId}")]

}