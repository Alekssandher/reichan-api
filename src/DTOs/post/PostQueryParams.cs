using MongoDB.Bson;
using MongoDB.Driver;

public class PostQueryParams {
    public int Limit { get; set; } = 20;
    public int Skip { get; set; } = 0;

    public string? Category { get; set; }
    public string? Author { get; set; }
    public PostQueryParams() {
        
        Limit = Math.Clamp(Limit, 1, 100);  
        Skip = Math.Max(Skip, 0);            
    }

    public FilterDefinition<BsonDocument> GetFilter () {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Empty;

        if (!string.IsNullOrEmpty(Category))
        {
            filter &= Builders<BsonDocument>.Filter.Eq("Category", Category);
        }

        if (!string.IsNullOrEmpty(Author))
        {
            filter &= Builders<BsonDocument>.Filter.Eq("Author", Author);
        }
        return filter;
    }
}
