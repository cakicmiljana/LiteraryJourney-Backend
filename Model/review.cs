namespace backend.model;
public class Review
{
    public int UserId { get; set; }
    public int ThemeId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}