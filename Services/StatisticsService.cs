using backend.model;
namespace backend.services;

public class StatisticsService{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _db;
    public StatisticsService(IMongoClient client)
    {
        _client = client;
        _db = _client.GetDatabase("Books");
    }

    public async Task<Statistics> CreateStatistics(string userId)
    {
        var statistics = new Statistics(userId);
        await _db.GetCollection<Statistics>("StatisticsCollection").InsertOneAsync(statistics);
        return statistics;
    }

    public async Task<Statistics> GetStatisticsByUserId(string userId)
    {
        var filter = Builders<Statistics>.Filter.Eq(s => s.UserId, new ObjectId(userId));
        var statistics = await _db.GetCollection<Statistics>("StatisticsCollection").Find(filter).FirstOrDefaultAsync();
        return statistics;
    }

    public async Task UpdateGenres(string userId, string genre)
    {
        var filter = Builders<Statistics>.Filter.Eq(s => s.UserId, new ObjectId(userId));
        var statistics = await _db.GetCollection<Statistics>("StatisticsCollection").Find(filter).FirstOrDefaultAsync();
        if (statistics.Genres.ContainsKey(genre))
        {
            statistics.Genres[genre]++;
        }
        else
        {
            statistics.Genres.Add(genre, 1);
        }
        await _db.GetCollection<Statistics>("StatisticsCollection").ReplaceOneAsync(filter, statistics);
    }

    public async Task UpdatePages(string userId, int pages)
    {
        var filter = Builders<Statistics>.Filter.Eq(s => s.UserId, new ObjectId(userId));
        var statistics = await _db.GetCollection<Statistics>("StatisticsCollection").Find(filter).FirstOrDefaultAsync();
        if (pages<200)
        {
            statistics.Pages["0-200"]++;
        }
        else if (pages<400)
        {
            statistics.Pages["200-400"]++;
        }
        else if (pages<700)
        {
            statistics.Pages["400-700"]++;
        }
        else
        {
            statistics.Pages["700+"]++;
        }
        await _db.GetCollection<Statistics>("StatisticsCollection").ReplaceOneAsync(filter, statistics);
    }

    public async Task UpdateLanguages(string userId, string language)
    {
        var filter = Builders<Statistics>.Filter.Eq(s => s.UserId, new ObjectId(userId));
        var statistics = await _db.GetCollection<Statistics>("StatisticsCollection").Find(filter).FirstOrDefaultAsync();
        if (statistics.Languages.ContainsKey(language))
        {
            statistics.Languages[language]++;
        }
        else
        {
            statistics.Languages.Add(language, 1);
        }
        await _db.GetCollection<Statistics>("StatisticsCollection").ReplaceOneAsync(filter, statistics);
    }

    public async Task UpdateAuthors(string userId, string author)
    {
        var filter = Builders<Statistics>.Filter.Eq(s => s.UserId, new ObjectId(userId));
        var statistics = await _db.GetCollection<Statistics>("StatisticsCollection").Find(filter).FirstOrDefaultAsync();
        if (statistics.Authors.ContainsKey(author))
        {
            statistics.Authors[author]++;
        }
        else
        {
            statistics.Authors.Add(author, 1);
        }
        await _db.GetCollection<Statistics>("StatisticsCollection").ReplaceOneAsync(filter, statistics);
    }
    public async Task DeleteStatistics(string userId)
    {
        var filter = Builders<Statistics>.Filter.Eq(s => s.UserId, new ObjectId(userId));
        await _db.GetCollection<Statistics>("StatisticsCollection").DeleteOneAsync(filter);
    }

}