using MongoDB.Bson.Serialization.Attributes;

namespace backend.model;

public class Book
{
    public ObjectId Id { get; set; }
    public int Pages { get; set; }
    public string Title { get; set; } = null!;
    public string Author { get; set; }  = null!;
    public string Language { get; set; } = null!;
    public string? Description { get; set; }
    public string? ExternalLink { get; set; }
    public string CoverPath { get; set; } = null!;
    public IEnumerable<string> Genres { get; set; } = new List<string>();
    public Theme? LiteraryTheme { get; set; }
}