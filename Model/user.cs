namespace backend.model;

public class User {
    public ObjectId Id { get; set; }
    public string UserType { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Country { get; set; } = null!;
    public IEnumerable<string> ThemeIDs { get; set; } = new List<string>();
    public IEnumerable<Book> Books { get; set; } = new List<Book>();
    public Statistics Statistics { get; set; } = new Statistics();
}