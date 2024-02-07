namespace backend.model;

public class Book
{
    public int Id { get; set; }
    public int Pages { get; set; }
    public string Title { get; set; } = null!;
    public string Author { get; set; }  = null!;
    public string Language { get; set; } = null!;
    public string ExternalLink { get; set; } = null!;
    public string CoverPath { get; set; } = null!;
    public IEnumerable<string> Genres { get; set; } = new List<string>();
    public Theme? LiteraryTheme { get; set; }
}