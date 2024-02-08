using backend.model;

namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _db;
    public StatisticsController(IMongoClient client)
    {
        _client = client;
        _db = _client.GetDatabase("Books");
    }

    [HttpPost("CreateStatistics/{userId}")]
    public async Task<ActionResult> CreateStatistics(string userId)
    {
        var statistics = new Statistics(userId);
        await _db.GetCollection<Statistics>("StatisticsCollection").InsertOneAsync(statistics);
        return Ok("Statistics created!");
    }

    [HttpGet("GetStatisticsByUserId/{userId}")]
    public async Task<ActionResult> GetStatisticsByUserId(string userId)
    {
        var filter = Builders<Statistics>.Filter.Eq(s => s.UserId, new ObjectId(userId));
        var statistics = await _db.GetCollection<Statistics>("StatisticsCollection").Find(filter).FirstOrDefaultAsync();
        return Ok(statistics);
    }

    [HttpPut("UpdateGenres/{userId}/{genre}")]
    public async Task<ActionResult> UpdateGenres(string userId, string genre)
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
        await _db.GetCollection<Statistics>("StatisticsCollection").ReplaceOneAsync(filter, statistics);
        return Ok("Genre updated!");
    }

    [HttpPut("UpdatePages/{userId}/{pages}")]
    public async Task<ActionResult> UpdatePages(string userId, int pages)
    {
        var filter = Builders<Statistics>.Filter.Eq(s => s.UserId, new ObjectId(userId));
        var statistics = await _db.GetCollection<Statistics>("StatisticsCollection").Find(filter).FirstOrDefaultAsync();
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
        await _db.GetCollection<Statistics>("StatisticsCollection").ReplaceOneAsync(filter, statistics);
        return Ok("Pages updated!");
    }

    [HttpPut("UpdateLanguages/{userId}/{language}")]
    public async Task<ActionResult> UpdateLanguages(string userId, string language)
    {
        var filter = Builders<Statistics>.Filter.Eq(s => s.UserId, new ObjectId(userId));
        var statistics = await _db.GetCollection<Statistics>("StatisticsCollection").Find(filter).FirstOrDefaultAsync();
        if (statistics.Languages.ContainsKey(language))
        {
            statistics.Languages[language]++;
        }
        else
        {
            statistics.Languages.Add(language, 1);
        }
        await _db.GetCollection<Statistics>("StatisticsCollection").ReplaceOneAsync(filter, statistics);
        return Ok("Language updated!");
    }

    [HttpPut("UpdateAuthors/{userId}/{author}")]
    public async Task<ActionResult> UpdateAuthors(string userId, string author)
    {
        var filter = Builders<Statistics>.Filter.Eq(s => s.UserId, new ObjectId(userId));
        var statistics = await _db.GetCollection<Statistics>("StatisticsCollection").Find(filter).FirstOrDefaultAsync();
        if (statistics.Authors.ContainsKey(author))
        {
            statistics.Authors[author]++;
        }
        else
        {
            statistics.Authors.Add(author, 1);
        }
        await _db.GetCollection<Statistics>("StatisticsCollection").ReplaceOneAsync(filter, statistics);
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
        var filter = Builders<Statistics>.Filter.Eq(s => s.UserId, new ObjectId(userId));
        await _db.GetCollection<Statistics>("StatisticsCollection").DeleteOneAsync(filter);
        return Ok("Statistics deleted!");
    }
}