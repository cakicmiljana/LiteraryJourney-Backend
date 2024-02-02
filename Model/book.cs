namespace backend.model;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Author { get; set; }  = null!;
    public string? ExternalLink { get; set; }
    public string CoverPath { get; set; } = null!;
    public Theme? LiteraryTheme { get; set; }
}