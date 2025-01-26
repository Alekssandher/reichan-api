using MongoDB.Bson;
using MongoDB.Driver;

public class FindUserByPublicKey {

    private readonly MongoClient client;
    private readonly IMongoCollection<BsonDocument> collection;

    public FindUserByPublicKey()
    {
        client = Database.Instance.GetClient();
        collection = client.GetDatabase("test").GetCollection<BsonDocument>("users");
    }

    public UserDto? FindUserByPubKey(string publicKey)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("PublicKey", publicKey);
        var user = collection.Find(filter).FirstOrDefault();
        
        return user == null ? null : new UserDto
        {
            Id = user["_id"].AsObjectId.ToString(),
            PublicKey = user["PublicKey"].AsString,
            Nick = user["Nick"].AsString,
            Image = user["Image"].AsString,
            Posts = user["Posts"].AsBsonArray.Select(p => p.AsString).ToArray()
        };
    }
}