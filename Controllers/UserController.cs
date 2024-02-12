using backend.dto;
using backend.model;
using backend.services;
namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    private readonly IMongoClient _client;
    private readonly IMongoDatabase _db;
    private readonly StatisticsService _statisticsServices;
    public UserController(IMongoClient client)
    {
        _client = client;
        _db = _client.GetDatabase("Books");
        _statisticsServices = new StatisticsService(_client);
    }

    [HttpPost("CreateUser")]
    public async Task<ActionResult> CreateUser([FromBody] User user)
    {
        try{var existingUser = await _db.GetCollection<User>("UserCollection").Find(u => u.Username == user.Username).FirstOrDefaultAsync();
        if (existingUser != null)
        {
            return BadRequest("User already exists!");
        }    
        user.Id = ObjectId.GenerateNewId();
        user.Statistics = await _statisticsServices.CreateStatistics(user.Id.ToString());
        await _db.GetCollection<User>("UserCollection").InsertOneAsync(user);
        return Ok("User created!" + user.Id);}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("User creation failed!");
        }
    }

    [HttpPost("LoginUser")]
    public async Task<ActionResult> Login([FromBody] LoginDTO loginUser)
    {
        try{var filter = Builders<User>.Filter.Eq(u => u.Username, loginUser.Username);
        var user = await _db.GetCollection<User>("UserCollection").Find(filter).FirstOrDefaultAsync();
        if (user == null)
        {
            return BadRequest("User not found!");
        }
        if (user.Password != loginUser.Password)
        {
            return BadRequest("Wrong password!");
        }
        return Ok(new {
            Id = user.Id.ToString(),
            user.Username,
            user.Password,
            user.Country,
            user.ThemeIDs,
            Books = user.Books.Select(b=>new{
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
            Statistics = new{
                Id = user.Statistics.Id.ToString(),
                user.Statistics.UserId,
                Genres = user.Statistics.Genres.OrderByDescending(g => g.Value).Take(5).ToDictionary(p => p.Key, p => p.Value),
                user.Statistics.Pages,
                user.Statistics.Languages,
                user.Statistics.Authors
            }
        });}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Login failed!");
        }
    }

    [HttpGet("GetUserById/{id}")]
    public async Task<ActionResult> GetUser(string id)
    {
        try{var filter = Builders<User>.Filter.Eq(b => b.Id, new ObjectId(id));
        var user = await _db.GetCollection<User>("UserCollection").Find(filter).FirstOrDefaultAsync();
        return Ok(new {
            Id = user.Id.ToString(),
            user.Username,
            user.Password,
            user.Country,
            user.ThemeIDs,
            Books = user.Books.Select(b=>new{
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
            Statistics = new{
                Id = user.Statistics.Id.ToString(),
                user.Statistics.UserId,
                user.Statistics.Genres,
                user.Statistics.Pages,
                user.Statistics.Languages,
                user.Statistics.Authors
            }
        });}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Get user failed");
        }
    }

    [HttpPut("UpdateUser/{id}/{country}")]
    public async Task<ActionResult> UpdateUser(string id, string country)
    {
        try{var filter = Builders<User>.Filter.Eq(b => b.Id, new ObjectId(id));
        var user = await _db.GetCollection<User>("UserCollection").Find(filter).FirstOrDefaultAsync();
        user.Country = country;
        await _db.GetCollection<User>("UserCollection").ReplaceOneAsync(filter, user);
        return Ok("User updated!");}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("User update failed!");
        }
    }

    [HttpPut("ChangePassword/{id}")]
    public async Task<ActionResult> ChangePassword(string id, [FromBody] string password)
    {
        try{var filter = Builders<User>.Filter.Eq(b => b.Id, new ObjectId(id));
        var user = await _db.GetCollection<User>("UserCollection").Find(filter).FirstOrDefaultAsync();
        user.Password = password;
        await _db.GetCollection<User>("UserCollection").ReplaceOneAsync(filter, user);
        return Ok("Password updated!");}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Password update failed!");
        }
    }

    [HttpDelete("DeleteUser/{id}")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        try{var filter = Builders<User>.Filter.Eq(b => b.Id, new ObjectId(id));
        await _db.GetCollection<User>("UserCollection").DeleteOneAsync(filter);
        return Ok("User deleted!");}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Delete user failed!");
        }
    }

    [HttpGet("GetAllUsers")]
    public async Task<ActionResult> GetAllUsers()
    {
        try{var users = await _db.GetCollection<User>("UserCollection").FindAsync(b => true).Result.ToListAsync();
        return Ok(users.Select(u => new {
            Id = u.Id.ToString(),
            u.Username,
            u.Password,
            u.Country,
            u.ThemeIDs,
            Books = u.Books.Select(b=>new{
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
            Statistics = new{
                Id = u.Statistics.Id.ToString(),
                u.Statistics.UserId,
                u.Statistics.Genres,
                u.Statistics.Pages,
                u.Statistics.Languages,
                u.Statistics.Authors
            }
        }).ToList());}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Get all users failed!");
        }
    }

    [HttpPut("ApplyForTheme/{userId}/{themeId}")]
    public async Task<ActionResult> ApplyForTheme(string userId, string themeId)
    {
        try{var userFilter = Builders<User>.Filter.Eq(u => u.Id, new ObjectId(userId));
        var user = await _db.GetCollection<User>("UserCollection").Find(userFilter).FirstOrDefaultAsync();
        if (user.ThemeIDs.Contains(themeId))
        {
            return BadRequest("User already applied for theme!");
        }
        var themeFilter = Builders<Theme>.Filter.Eq(t => t.Id, new ObjectId(themeId));
        var theme = await _db.GetCollection<Theme>("ThemeCollection").Find(themeFilter).FirstOrDefaultAsync();
        var updateFilter = Builders<User>.Update.Push<string>(p => p.ThemeIDs, themeId);
        await _db.GetCollection<User>("UserCollection").UpdateOneAsync(userFilter, updateFilter);
        return Ok("Applied for theme!");}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Apply for theme failed!");
        }
    }

    [HttpGet("GetAllThemesForUser/{userId}")]
    public async Task<ActionResult> GetAllThemesForUser(string userId)
    {
        try{var userFilter = Builders<User>.Filter.Eq(u => u.Id, new ObjectId(userId));
        var user = await _db.GetCollection<User>("UserCollection").Find(userFilter).FirstOrDefaultAsync();
        var themes = await _db.GetCollection<Theme>("ThemeCollection").Find(t => user.ThemeIDs.Contains(t.Id.ToString())).ToListAsync();
        return Ok(themes.Select(b=>new{
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
            return BadRequest("Get all themes for user failed!");
        }
    }

    [HttpPut("ReadBook/{userId}/{bookId}")]
    public async Task<ActionResult> ReadBook(string userId, string bookId)
    {
        try{var userFilter = Builders<User>.Filter.Eq(u => u.Id, new ObjectId(userId));
        var user = await _db.GetCollection<User>("UserCollection").Find(userFilter).FirstOrDefaultAsync();
        if (user.Books.Any(b => b.Id.ToString() == bookId))
        {
            return BadRequest("User already read book!");
        }
        var bookFilter = Builders<Book>.Filter.Eq(b => b.Id, new ObjectId(bookId));
        var book = await _db.GetCollection<Book>("BookCollection").Find(bookFilter).FirstOrDefaultAsync();
        var updateFilter = Builders<User>.Update.Push<Book>(p => p.Books, book);
        await _statisticsServices.UpdateStatistics(userId, book.Genres, book.Pages, book.Language, book.Author);
        await _db.GetCollection<User>("UserCollection").UpdateOneAsync(userFilter, updateFilter);
        return Ok("Book read!");}
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("Read book failed!");
        }
    }
}