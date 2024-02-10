using backend.model;

namespace backend.services;

public class ThemeService {


    private readonly IMongoClient _client;
    private readonly IMongoDatabase _db;
    public ThemeService(IMongoClient client)
    {
        _client = client;
        _db = _client.GetDatabase("Books");
    }

    public async Task AddGenresToTheme(Book book, FilterDefinition<Theme> themeFilter){

        var theme = await _db.GetCollection<Theme>("ThemeCollection").Find(themeFilter).FirstOrDefaultAsync();
        int count = theme.Genres.Count;
        theme.Genres.UnionWith(book.Genres);
        if(theme.Genres.Count > count){
            var updateGenres = Builders<Theme>.Update.PushEach<string>(p=>p.Genres, theme.Genres);
            await _db.GetCollection<Theme>("ThemeCollection").UpdateOneAsync(themeFilter, updateGenres);
        }
    }

}