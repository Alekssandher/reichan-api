using MongoDB.Bson;
using MongoDB.Driver;

public class ReplyQueryParams {
    public int Limit { get; set; } = 20;
    public int Skip { get; set; } = 0;

    public string? PostId { get; set; }
    public ReplyQueryParams() {
        
        Limit = Math.Clamp(Limit, 1, 50);  
        Skip = Math.Max(Skip, 0);            
    }

    public FilterDefinition<BsonDocument> GetFilter () {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Empty;

        if (!string.IsNullOrEmpty(PostId))
        {
            filter &= Builders<BsonDocument>.Filter.Eq("PostId", PostId);
        }
        return filter;
    }
}
