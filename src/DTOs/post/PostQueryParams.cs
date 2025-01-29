using MongoDB.Bson;
using MongoDB.Driver;

public class PostQueryParams {
    public int Limit { get; set; } = 20;
    public int Skip { get; set; } = 0;

    public string? Category { get; set; }
    public string? Author { get; set; }
    public PostQueryParams() {
        
        Limit = Math.Clamp(Limit, 1, 50);  
        Skip = Math.Max(Skip, 0);            
    }

    public FilterDefinition<BsonDocument> GetFilter () {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Empty;

        if (!string.IsNullOrEmpty(Category))
        {
            filter &= Builders<BsonDocument>.Filter.Eq("Category", Category.ToLower());
        }

        if (!string.IsNullOrEmpty(Author))
        {
            filter &= Builders<BsonDocument>.Filter.Eq("Author", Author);
        }
        return filter;
    }

    public FindOptions<BsonDocument> GetFindOptions() {
        return new FindOptions<BsonDocument> {
            Skip = Skip,
            Limit = Limit,
            Sort = Builders<BsonDocument>.Sort.Descending("CreatedAt") 
        };
    }
}
