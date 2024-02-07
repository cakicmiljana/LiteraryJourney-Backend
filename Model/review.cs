namespace backend.model;
public class Review
{
    public ObjectId Id { get; set; }
    public int UserId { get; set; }
    public int ThemeId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = null!;
}