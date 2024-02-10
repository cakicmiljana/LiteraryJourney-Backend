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

    [HttpGet("GetRecommendationByLanguage/{userId}")]
    public async Task<ActionResult> GetRecommendationByLanguage(string userId)
    {
        var statistics = await _statisticsServices.GetStatisticsByUserId(userId);
        string favouriteLanguage = statistics.Languages.Where(p => p.Value == statistics.Languages.Values.Max()).Select(p => p.Key).FirstOrDefault();
        if (string.IsNullOrWhiteSpace(favouriteLanguage))
        {
            return BadRequest("User has no favourite language!");
        }
        var filter = Builders<Book>.Filter.Eq(b => b.Language, favouriteLanguage);
        var books = await _db.GetCollection<Book>("BookCollection").Find(filter).ToListAsync();
        return Ok(books);
    }

    [HttpGet("GetRecommendationByAuthor/{userId}")]
    public async Task<ActionResult> GetRecommendationByAuthor(string userId)
    {
        var statistics = await _statisticsServices.GetStatisticsByUserId(userId);
        string favouriteAuthor = statistics.Authors.Where(p => p.Value == statistics.Authors.Values.Max()).Select(p => p.Key).FirstOrDefault();
        if (string.IsNullOrWhiteSpace(favouriteAuthor))
        {
            return BadRequest("User has no favourite author!");
        }
        var filter = Builders<Book>.Filter.Eq(b => b.Author, favouriteAuthor);
        var books = await _db.GetCollection<Book>("BookCollection").Find(filter).ToListAsync();
        return Ok(books.Select(b=> new {
            Id = b.Id.ToString(),
            b.Pages,
            b.Title,
            b.Author,
            b.CoverPath,
            b.Description,
            b.ExternalLink,
            b.Genres,
            b.Language,
        }));
    }
    [HttpGet("GetRecommendationByGenre/{userId}")]
    public async Task<ActionResult> GetRecommendationByGenre(string userId)
    {
        var statistics = await _statisticsServices.GetStatisticsByUserId(userId);
        string favouriteGenre = statistics.Genres.Where(p => p.Value == statistics.Genres.Values.Max()).Select(p => p.Key).FirstOrDefault();
        if (string.IsNullOrWhiteSpace(favouriteGenre))
        {
            return BadRequest("User has no favourite genre!");
        }
        var books = await _db.GetCollection<Book>("BookCollection").Find(b=>b.Genres.Contains(favouriteGenre)).ToListAsync();
        return Ok(books);
    }

}