using MongoDB.Bson.Serialization.Attributes;

namespace backend.model;

public class Theme
{
    public ObjectId Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImagePath { get; set; } = null!;
    public decimal? Rating { get; set; } 
    public IEnumerable<Book> Books { get; set; } = new List<Book>();
    public IEnumerable<string> Genres { get; set; } = new List<string>();
    public IEnumerable<Review> Reviews { get; set; } = new List<Review>();
}