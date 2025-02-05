using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

public class UserService : IUserService
{
    private readonly MongoClient client;
    private readonly IMongoCollection<BsonDocument> collection;

    public UserService(DatabaseConfig config)
    {
        client = Database.Instance.GetClient();
        collection = client.GetDatabase(config.DatabaseName).GetCollection<BsonDocument>(config.UsersCollection);
    }
    public async Task<List<UserDto>> GetAllAsync()
    {   
        FilterDefinition<BsonDocument> filter = FilterDefinition<BsonDocument>.Empty;

        List<BsonDocument> users = await collection.Find(filter).ToListAsync();
        List<UserDto> userDtos = users.Select(user => BsonSerializer.Deserialize<UserDto>(user)).ToList();
        
        return userDtos;
    }

    public async Task CreateAsync(CreateUserDto body)
    {
        UserDto? userExists = await new FindUserByPublicKey().FindUserByPubKey(body.PublicKey);

        if (userExists != null)
        {
            throw new InvalidOperationException("This user already exists");
        }

        var user = new CreateUserDto(body.Nick, body.PublicKey, body.Image).ToBsonDocument();

        await collection.InsertOneAsync(user);
        
        return;
    }
}
