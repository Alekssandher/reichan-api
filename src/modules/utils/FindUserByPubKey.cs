using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

public class FindUserByPublicKey {

    private readonly MongoClient client;
    private readonly IMongoCollection<BsonDocument> collection;

    public FindUserByPublicKey()
    {
        client = Database.Instance.GetClient();
        collection = client.GetDatabase("test").GetCollection<BsonDocument>("users");
    }

    public async Task<UserDto?>  FindUserByPubKey(string publicKey)
    {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("PublicKey", publicKey);
        BsonDocument userBson = await collection.Find(filter).FirstOrDefaultAsync();

        if(userBson == null) return null;
        UserDto userDto = BsonSerializer.Deserialize<UserDto>(userBson);
        
        return userDto;
    }
}