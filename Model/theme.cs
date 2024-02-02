using System.Globalization;

namespace backend.model;

public class Theme
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
    public Book[]? Books { get; set; }
    public decimal? Rating { get; set; }
}