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
    public async Task<ActionResult> UpdateStatistics(string userId, string genre, int pages, string language, string author)
    {
        var filter = Builders<Statistics>.Filter.Eq(s => s.UserId, new ObjectId(userId));
        var statistics = await _db.GetCollection<Statistics>("StatisticsCollection").Find(filter).FirstOrDefaultAsync();
        if (statistics.Genres.ContainsKey(genre))
        {
            statistics.Genres[genre]++;
        }
        else
        {
            statistics.Genres.Add(genre, 1);
        }
        if (pages<200)
        {
            statistics.Pages["0-200"]++;
        }
        else if (pages<400)
        {
            statistics.Pages["200-400"]++;
        }
        else if (pages<700)
        {
            statistics.Pages["400-700"]++;
        }
        else
        {
            statistics.Pages["700+"]++;
        }

        if (statistics.Languages.ContainsKey(language))
        {
            statistics.Languages[language]++;
        }
        else
        {
            statistics.Languages.Add(language, 1);
        }

        if (statistics.Authors.ContainsKey(author))
        {
            statistics.Authors[author]++;
        }
        else
        {
            statistics.Authors.Add(author, 1);
        }
        await _db.GetCollection<Statistics>("StatisticsCollection").ReplaceOneAsync(filter, statistics);
        return Ok("Statistics updated!");
    }

    [HttpDelete("DeleteStatistics/{userId}")]
    public async Task<ActionResult> DeleteStatistics(string userId)
    {
        await _statisticsService.DeleteStatistics(userId);
        return Ok("Statistics deleted!");
    }
}