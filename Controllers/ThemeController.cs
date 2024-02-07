using backend.model;

namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class ThemeController : ControllerBase
{
    
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _db;
    public ThemeController(IMongoClient client)
    {
        _client = client;
        _db = _client.GetDatabase("Books");
    }

    [HttpPost("CreateTheme")]
    public async Task<ActionResult> CreateTheme([FromBody] Theme theme)
    {
        theme.Id = ObjectId.GenerateNewId();
        await _db.GetCollection<Theme>("ThemeCollection").InsertOneAsync(theme);
        return Ok("Theme created with id " + theme.Id);
    }

    [HttpGet("GetThemeById/{id}")]
    public async Task<ActionResult> GetThemeById(string id)
    {
        var filter = Builders<Theme>.Filter.Eq(t => t.Id, new ObjectId(id));
        var theme = await _db.GetCollection<Theme>("ThemeCollection").Find(filter).FirstOrDefaultAsync();
        return Ok(theme);
    }

    [HttpPut("UpdateThemeDescription/{id}/{description}")]
    public async Task<ActionResult> UpdateThemeDescription(string id, string description)
    {
        var filter = Builders<Theme>.Filter.Eq(t => t.Id, new ObjectId(id));
        var theme = await _db.GetCollection<Theme>("ThemeCollection").Find(filter).FirstOrDefaultAsync();
        
        theme.Description = description;
        await _db.GetCollection<Theme>("ThemeCollection").ReplaceOneAsync(filter, theme);
        
        return Ok("Theme description updated!");
    }

    [HttpDelete("DeleteTheme/{id}")]
    public async Task<ActionResult> DeleteTheme(string id)
    {
        var filter = Builders<Theme>.Filter.Eq(t => t.Id, new ObjectId(id));
        await _db.GetCollection<Theme>("ThemeCollection").DeleteOneAsync(filter);
        return Ok("Theme deleted!");
    }

    [HttpGet("GetAllBooksInTheme/{id}")]
    public async Task<ActionResult> GetAllBooksInTheme(string id)
    {
        var filter = Builders<Theme>.Filter.Eq(t => t.Id, new ObjectId(id));
        var books = await _db.GetCollection<Theme>("ThemeCollection").Find(filter).FirstOrDefaultAsync();
        return Ok(books.Books);
    }
    [HttpGet("GetAllThemes")]
    public async Task<ActionResult> GetAllThemes()
    {
        var themes = await _db.GetCollection<Theme>("ThemeCollection").Find(_ => true).ToListAsync();
        return Ok(themes);
    }

    [HttpPut("AddBookToTheme/{themeId}/{bookId}")]
    public async Task<ActionResult> AddBookToTheme(string themeId, string bookId)
    {

        var themeFilter = Builders<Theme>.Filter.Eq(t => t.Id, new ObjectId(themeId));
        var bookFilter = Builders<Book>.Filter.Eq(b => b.Id, new ObjectId(bookId));
        var book = await _db.GetCollection<Book>("BookCollection").Find(bookFilter).FirstOrDefaultAsync();
        var updateFilter = Builders<Theme>.Update.Push<Book>(p=>p.Books, book);
        await _db.GetCollection<Theme>("ThemeCollection").UpdateOneAsync(themeFilter, updateFilter);
        
        return Ok("Book added to theme!");
    }
}