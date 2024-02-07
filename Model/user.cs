namespace backend.model;

public class User {
    public int Id { get; set; }
    public string UserType { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Country { get; set; } = null!;
    public IEnumerable<Theme> Themes { get; set; } = new List<Theme>();
    public IEnumerable<int> BookPages { get; set; } = new List<int>();
    public IEnumerable<string> Genres { get; set; } = new List<string>();
}