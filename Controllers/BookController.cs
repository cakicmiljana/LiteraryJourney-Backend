using backend.model;

namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _db;
    
    public BookController(IMongoClient client)
    {
        _client = client;
        _db = _client.GetDatabase("Books");
    }

    [HttpPost("CreateBook")]
    public async Task<ActionResult> CreateBook([FromBody] Book book)
    {
        book.Id = ObjectId.GenerateNewId();
        await _db.GetCollection<Book>("BookCollection").InsertOneAsync(book);
        return Ok("Book created! ID: " + book.Id);
    }

    [HttpGet("GetBookById/{id}")]
    public async Task<ActionResult> GetBook(string id)
    {
        var filter = Builders<Book>.Filter.Eq(b => b.Id, new ObjectId(id));
        var book = await _db.GetCollection<Book>("BookCollection").Find(filter).FirstOrDefaultAsync();
        return Ok(new {
            Id = book.Id.ToString(),
            book.Pages,
            book.Title,
            book.Author,
            book.CoverPath,
            book.Description,
            book.ExternalLink,
            book.Genres,
            book.Language
        });
    }

    [HttpPut("UpdateBookPagesLanguage/{id}/{pages}/{language}")]
    public async Task<ActionResult> UpdateBook(string id, int pages, string language)
    {
        var filter = Builders<Book>.Filter.Eq(b => b.Id, new ObjectId(id));
        var book = await _db.GetCollection<Book>("BookCollection").Find(filter).FirstOrDefaultAsync();
        book.Pages = pages;
        book.Language = language;
        await _db.GetCollection<Book>("BookCollection").ReplaceOneAsync(filter, book);
        return Ok("Book updated!");
    }

    [HttpDelete("DeleteBook/{id}")]
    public async Task<ActionResult> DeleteBook(string id)
    {
        var filter = Builders<Book>.Filter.Eq(b => b.Id, new ObjectId(id));
        await _db.GetCollection<Book>("BookCollection").DeleteOneAsync(filter);
        return Ok("Book deleted!");
    }

    [HttpGet("GetBookByTitle/{title}")]
    public async Task<ActionResult> GetBookByTitle(string title)
    {
        var book = await _db.GetCollection<Book>("BookCollection").FindAsync(b => b.Title == title).Result.FirstOrDefaultAsync();
        return Ok(new {
            Id = book.Id.ToString(),
            book.Pages,
            book.Title,
            book.Author,
            book.CoverPath,
            book.Description,
            book.ExternalLink,
            book.Genres,
            book.Language
        });
    }

    [HttpGet("GetAllBooksByGenre/{genre}")]
    public async Task<ActionResult> GetAllBooksByGenre(string genre)
    {
        var books = await _db.GetCollection<Book>("BookCollection").FindAsync(b => b.Genres.Contains(genre)).Result.ToListAsync();
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
        }).ToList());
    }

    [HttpGet("GetAllBooks")]
    public async Task<ActionResult> GetAllBooks()
    {
        var books = await _db.GetCollection<Book>("BookCollection").Find(b => true).ToListAsync();
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
        }).ToList());
    }
    
}