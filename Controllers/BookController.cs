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
        await _db.GetCollection<Book>("Books").InsertOneAsync(book);
        return Ok("Book created! ID: " + book.Id);
    }

    [HttpGet("GetBook/{id}")]
    public async Task<ActionResult> GetBook(ObjectId id)
    {
        var book = await _db.GetCollection<Book>("Books").FindAsync(b => b.Id == id).Result.FirstOrDefaultAsync();
        return Ok(book);
    }

    [HttpPut("UpdateBook")]
    public async Task<ActionResult> UpdateBook([FromBody] Book book)
    {
        return Ok("User updated!");
    }

    [HttpGet("GetBookByTitle/{title}")]
    public async Task<ActionResult> GetBook(string title)
    {
        var book = await _db.GetCollection<Book>("Books").FindAsync(b => b.Title == title).Result.FirstOrDefaultAsync();
        return Ok(book);
    }

    [HttpGet("GetAllBooksByGenre/{genre}")]
    public async Task<ActionResult> GetAllBooksByGenre(string genre)
    {
        var books = await _db.GetCollection<Book>("Books").FindAsync(b => b.Genres.Contains(genre)).Result.ToListAsync();
        return Ok(books);
    }
}