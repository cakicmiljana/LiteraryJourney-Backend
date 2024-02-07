namespace backend.model;
public class Review
{
    public ObjectId Id { get; set; }
    public string UserId { get; set; }= null!;
    public string ThemeId { get; set; } = null!;
    public int Rating { get; set; }
    public string Comment { get; set; } = null!;
}