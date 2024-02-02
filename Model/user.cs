namespace backend.model;

public class User {
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Country { get; set; } = null!;
}