namespace backend.model;

public class Theme
{
    public ObjectId Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public IEnumerable<Book> Books { get; set; } = new List<Book>();
    public IEnumerable<string> Genres { get; set; } = new List<string>();
    public decimal? Rating { get; set; }
}