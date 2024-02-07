namespace backend.model;
public class Review
{
    private ObjectId Id { get; set; }
    public int UserId { get; set; }
    public int ThemeId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}