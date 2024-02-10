using backend.model;
using backend.services;

namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class ThemeController : ControllerBase
{
    
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _db;
    private readonly ThemeService _themeService;
    public ThemeController(IMongoClient client)
    {
        _client = client;
        _db = _client.GetDatabase("Books");
        _themeService = new ThemeService(_client);
    }

    [HttpPost("CreateTheme")]
    public async Task<ActionResult> CreateTheme([FromBody] Theme theme)
    {
        try{theme.Id = ObjectId.GenerateNewId();
        await _db.GetCollection<Theme>("ThemeCollection").InsertOneAsync(theme);
        return Ok("Theme created with id " + theme.Id);}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Theme creation failed!");
        }
    }

    [HttpGet("GetThemeById/{id}")]
    public async Task<ActionResult> GetThemeById(string id)
    {
        try{var filter = Builders<Theme>.Filter.Eq(t => t.Id, new ObjectId(id));
        var theme = await _db.GetCollection<Theme>("ThemeCollection").Find(filter).FirstOrDefaultAsync();
        return Ok(new {
            Id = theme.Id.ToString(),
            theme.Title,
            theme.Description,
            theme.ImagePath,
            theme.Rating,
            Books = theme.Books.Select(b=>new{
                Id = b.Id.ToString(),
                b.Pages,
                b.Title,
                b.Author,
                b.CoverPath,
                b.Description,
                b.ExternalLink,
                b.Genres,
                b.Language
            }).ToList(),
            theme.Genres,
            Reviews = theme.Reviews.Select(b=>new{
                Id = b.Id.ToString(),
                b.UserId,
                b.ThemeId,
                b.Rating,
                b.Comment
            }).ToList()
        });}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Theme get failed!");
        }
    }

    [HttpPut("UpdateThemeDescription/{id}/{description}")]
    public async Task<ActionResult> UpdateThemeDescription(string id, string description)
    {
        try{var filter = Builders<Theme>.Filter.Eq(t => t.Id, new ObjectId(id));
        var theme = await _db.GetCollection<Theme>("ThemeCollection").Find(filter).FirstOrDefaultAsync();
        
        theme.Description = description;
        await _db.GetCollection<Theme>("ThemeCollection").ReplaceOneAsync(filter, theme);
        
        return Ok("Theme description updated!");}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Theme description update failed!");
        }
    }

    [HttpDelete("DeleteTheme/{id}")]
    public async Task<ActionResult> DeleteTheme(string id)
    {
        try{var filter = Builders<Theme>.Filter.Eq(t => t.Id, new ObjectId(id));
        await _db.GetCollection<Theme>("ThemeCollection").DeleteOneAsync(filter);
        return Ok("Theme deleted!");}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Delete theme failed!");
        }
    }

    [HttpGet("GetAllBooksInTheme/{id}")]
    public async Task<ActionResult> GetAllBooksInTheme(string id)
    {
        try{var filter = Builders<Theme>.Filter.Eq(t => t.Id, new ObjectId(id));
        var books = await _db.GetCollection<Theme>("ThemeCollection").Find(filter).FirstOrDefaultAsync();
        return Ok(books.Books.Select(b=>new{
            Id = b.Id.ToString(),
            b.Pages,
            b.Title,
            b.Author,
            b.CoverPath,
            b.Description,
            b.ExternalLink,
            b.Genres,
            b.Language
        }).ToList());}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Get books in theme failed!");
        }
    }
    [HttpGet("GetAllThemes")]
    public async Task<ActionResult> GetAllThemes()
    {
        try{var themes = await _db.GetCollection<Theme>("ThemeCollection").Find(_ => true).ToListAsync();
        return Ok(themes.Select(b => new {
            Id = b.Id.ToString(),
            b.Title,
            b.Description,
            b.ImagePath,
            b.Rating,
            Books = b.Books.Select(b=>new{
                Id = b.Id.ToString(),
                b.Pages,
                b.Title,
                b.Author,
                b.CoverPath,
                b.Description,
                b.ExternalLink,
                b.Genres,
                b.Language
            }).ToList(),
            b.Genres,
            Reviews = b.Reviews.Select(p=>new{
                Id = p.Id.ToString(),
                p.UserId,
                p.ThemeId,
                p.Rating,
                p.Comment
            }).ToList()
        }).ToList());}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Get all themes failed!");
        }
    }

    [HttpPut("AddBookToTheme/{themeId}/{bookId}")]
    public async Task<ActionResult> AddBookToTheme(string themeId, string bookId)
    {

        try{var themeFilter = Builders<Theme>.Filter.Eq(t => t.Id, new ObjectId(themeId));
        var bookFilter = Builders<Book>.Filter.Eq(b => b.Id, new ObjectId(bookId));
        var book = await _db.GetCollection<Book>("BookCollection").Find(bookFilter).FirstOrDefaultAsync();
        var updateFilter = Builders<Theme>.Update.Push<Book>(p=>p.Books, book);
        await _db.GetCollection<Theme>("ThemeCollection").UpdateOneAsync(themeFilter, updateFilter);

        await _themeService.AddGenresToTheme(book, themeFilter);
        
        return Ok("Book added to theme!");}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Add book to theme failed!");
        }
    }

    [HttpPut("RemoveBookFromTheme/{themeId}/{bookId}")]
    public async Task<ActionResult> RemoveBookFromTheme(string themeId, string bookId)
    {
        try{var themeFilter = Builders<Theme>.Filter.Eq(t => t.Id, new ObjectId(themeId));
        var bookFilter = Builders<Book>.Filter.Eq(b => b.Id, new ObjectId(bookId));
        var book = await _db.GetCollection<Book>("BookCollection").Find(bookFilter).FirstOrDefaultAsync();
        var updateFilter = Builders<Theme>.Update.PullFilter(p=>p.Books, bookFilter);
        await _db.GetCollection<Theme>("ThemeCollection").UpdateOneAsync(themeFilter, updateFilter);
        return Ok("Book removed from theme!");}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Remove book from theme failed!");
        }
    }

    // //for existing theme that has books but no genres are added to the theme
    // [HttpPut("AddGenresToTheme/{themeId}")]
    // public async Task<ActionResult> AddGenresToTheme(string themeId)
    // {
    //     try{var theme = await _db.GetCollection<Theme>("ThemeCollection").Find(t => t.Id == new ObjectId(themeId)).FirstOrDefaultAsync();
    //     var themeFilter = Builders<Theme>.Filter.Eq(t => t.Id, new ObjectId(themeId));
    //     var updateFilter = Builders<Theme>.Update.PushEach<string>(p=>p.Genres, theme.Books.SelectMany(b=>b.Genres).Distinct());
    //     await _db.GetCollection<Theme>("ThemeCollection").UpdateOneAsync(themeFilter, updateFilter);
    //     return Ok("Genres added to theme!");}
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         return BadRequest("Add genres to theme failed!");
    //     }
    // }

    [HttpGet("GetReviews/{themeId}")]
    public async Task<ActionResult> GetReviews(string themeId)
    {
        try{var theme = await _db.GetCollection<Theme>("ThemeCollection").Find(t => t.Id == new ObjectId(themeId)).FirstOrDefaultAsync();
        return Ok(theme.Reviews.Select(b=>new{
            Id = b.Id.ToString(),
            b.UserId,
            b.ThemeId,
            b.Rating,
            b.Comment
        }));}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Get reviews failed!");
        }
    }
}