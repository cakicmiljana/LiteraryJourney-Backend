namespace backend.model;

public class Statistics
{
    public ObjectId UserId { get; set; }
    public Dictionary<string, int> Genres { get; set; } = new Dictionary<string, int>();
    public Dictionary<string, int> Pages { get; set; } = new Dictionary<string, int>();
    public Dictionary<string, int> Authors { get; set; } = new Dictionary<string, int>();
    public Dictionary<string, int> Languages { get; set; } = new Dictionary<string, int>();

    public Statistics(){
        Pages.Add("0-200", 0);
        Pages.Add("200-400", 0);
        Pages.Add("400-700", 0);
        Pages.Add("700+", 0);
    }
    public Statistics(string userId)
    {
        UserId = new ObjectId(userId);
        Pages.Add("0-200", 0);
        Pages.Add("200-400", 0);
        Pages.Add("400-700", 0);
        Pages.Add("700+", 0);
    }
}