using backend.model;

namespace backend.controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    private readonly IMongoClient _client;
    private readonly IMongoDatabase _db;
    public UserController(IMongoClient client)
    {
        _client = client;
        _db = _client.GetDatabase("Books");
    }

    [HttpPost("CreateUser")]
    public async Task<ActionResult> CreateUser([FromBody] User user)
    {
        user.Id = ObjectId.GenerateNewId();
        await _db.GetCollection<User>("UserCollection").InsertOneAsync(user);
        return Ok("User created!" + user.Id);
    }

    [HttpGet("GetUserById/{id}")]
    public async Task<ActionResult> GetUser(string id)
    {
        var filter = Builders<User>.Filter.Eq(b => b.Id, new ObjectId(id));
        var user = await _db.GetCollection<User>("UserCollection").Find(filter).FirstOrDefaultAsync();
        return Ok(user);
    }

    [HttpPut("UpdateUser/{id}/{country}")]
    public async Task<ActionResult> UpdateUser(string id, string country)
    {
        var filter = Builders<User>.Filter.Eq(b => b.Id, new ObjectId(id));
        var user = await _db.GetCollection<User>("UserCollection").Find(filter).FirstOrDefaultAsync();
        user.Country = country;
        await _db.GetCollection<User>("UserCollection").ReplaceOneAsync(filter, user);
        return Ok("User updated!");
    }

    [HttpPut("ChangePassword/{id}")]
    public async Task<ActionResult> ChangePassword(string id, [FromBody] string password)
    {
        var filter = Builders<User>.Filter.Eq(b => b.Id, new ObjectId(id));
        var user = await _db.GetCollection<User>("UserCollection").Find(filter).FirstOrDefaultAsync();
        user.Password = password;
        await _db.GetCollection<User>("UserCollection").ReplaceOneAsync(filter, user);
        return Ok("Password updated!");
    }

    [HttpDelete("DeleteUser/{id}")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        var filter = Builders<User>.Filter.Eq(b => b.Id, new ObjectId(id));
        await _db.GetCollection<User>("UserCollection").DeleteOneAsync(filter);
        return Ok("User deleted!");
    }

    [HttpGet("GetAllUsers")]
    public async Task<ActionResult> GetAllUsers()
    {
        var users = await _db.GetCollection<User>("UserCollection").FindAsync(b => true).Result.ToListAsync();
        return Ok(users);
    }
}