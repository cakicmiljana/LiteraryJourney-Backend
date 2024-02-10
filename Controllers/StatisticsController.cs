using backend.model;
using backend.services;
namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _db;
    private readonly StatisticsService _statisticsService;
    public StatisticsController(IMongoClient client)
    {
        _client = client;
        _db = _client.GetDatabase("Books");
        _statisticsService = new StatisticsService(_client);
    }

    [HttpPost("CreateStatistics/{userId}")]
    public async Task<ActionResult> CreateStatistics(string userId)
    {
        await _statisticsService.CreateStatistics(userId);
        return Ok("Statistics created!");
    }

    [HttpGet("GetStatisticsByUserId/{userId}")]
    public async Task<ActionResult> GetStatisticsByUserId(string userId)
    {
        Statistics s = await _statisticsService.GetStatisticsByUserId(userId);
        return Ok(s);
    }

    [HttpPut("UpdateGenres/{userId}/{genre}")]
    public async Task<ActionResult> UpdateGenres(string userId, string genre)
    {
        await _statisticsService.UpdateGenres(userId, genre);
        return Ok("Genre updated!");
    }

    [HttpPut("UpdatePages/{userId}/{pages}")]
    public async Task<ActionResult> UpdatePages(string userId, int pages)
    {
        await _statisticsService.UpdatePages(userId, pages);
        return Ok("Pages updated!");
    }

    [HttpPut("UpdateLanguages/{userId}/{language}")]
    public async Task<ActionResult> UpdateLanguages(string userId, string language)
    {
        await _statisticsService.UpdateLanguages(userId, language);
        return Ok("Language updated!");
    }

    [HttpPut("UpdateAuthors/{userId}/{author}")]
    public async Task<ActionResult> UpdateAuthors(string userId, string author)
    {
        await _statisticsService.UpdateAuthors(userId, author);
        return Ok("Author updated!");
    }

    [HttpPut("UpdateStatistics/{userId}/{genre}/{pages}/{language}/{author}")]
    public async Task<ActionResult> UpdateStatistics(string userId, IEnumerable<string> genres, int pages, string language, string author)
    {
        await _statisticsService.UpdateStatistics(userId, genres, pages, language, author);
        return Ok("Statistics updated!");
    }

    [HttpDelete("DeleteStatistics/{userId}")]
    public async Task<ActionResult> DeleteStatistics(string userId)
    {
        await _statisticsService.DeleteStatistics(userId);
        return Ok("Statistics deleted!");
    }
}