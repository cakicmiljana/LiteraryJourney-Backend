namespace backend.dto;

public class BookDTO
{
    public string? Id { get; set; }
    public int Pages { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? CoverPath { get; set; }
    public string? Description { get; set; }
    public string? ExternalLink { get; set; }
    public IEnumerable<string> Genres { get; set; } = new List<string>();
    public string? Language { get; set; }
}