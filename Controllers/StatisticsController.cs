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
        try{await _statisticsService.CreateStatistics(userId);
        return Ok("Statistics created!");}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Statistics creation failed!");
        }
    }

    [HttpGet("GetStatisticsByUserId/{userId}")]
    public async Task<ActionResult> GetStatisticsByUserId(string userId)
    {
        try{Statistics s = await _statisticsService.GetStatisticsByUserId(userId);
        return Ok(new{
            Id = s.Id.ToString(),
            s.UserId,
            s.Genres,
            s.Pages,
            s.Languages,
            s.Authors
        });}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Get statistics failed!");
        }
    }

    [HttpPut("UpdateGenres/{userId}/{genre}")]
    public async Task<ActionResult> UpdateGenres(string userId, string genre)
    {
        try{await _statisticsService.UpdateGenres(userId, genre);
        return Ok("Genre updated!");}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Statistics - Genre update failed!");
        }
    }

    [HttpPut("UpdatePages/{userId}/{pages}")]
    public async Task<ActionResult> UpdatePages(string userId, int pages)
    {
        try{await _statisticsService.UpdatePages(userId, pages);
        return Ok("Pages updated!");}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Statistics - Pages update failed!");
        }
    }

    [HttpPut("UpdateLanguages/{userId}/{language}")]
    public async Task<ActionResult> UpdateLanguages(string userId, string language)
    {
        try{await _statisticsService.UpdateLanguages(userId, language);
        return Ok("Language updated!");}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Statistics - Language update failed!");
        }
    }

    [HttpPut("UpdateAuthors/{userId}/{author}")]
    public async Task<ActionResult> UpdateAuthors(string userId, string author)
    {
        try{await _statisticsService.UpdateAuthors(userId, author);
        return Ok("Author updated!");}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Statistics - Author update failed!");
        }
    }

    [HttpPut("UpdateStatistics/{userId}/{genre}/{pages}/{language}/{author}")]
    public async Task<ActionResult> UpdateStatistics(string userId, IEnumerable<string> genres, int pages, string language, string author)
    {
        try{await _statisticsService.UpdateStatistics(userId, genres, pages, language, author);
        return Ok("Statistics updated!");}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Statistics update failed!");
        }
    }

    [HttpDelete("DeleteStatistics/{userId}")]
    public async Task<ActionResult> DeleteStatistics(string userId)
    {
        try{await _statisticsService.DeleteStatistics(userId);
        return Ok("Statistics deleted!");}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Statistics delete failed!");
        }
    }
}