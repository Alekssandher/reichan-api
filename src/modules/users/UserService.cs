using System.Text.Json;
using MongoDB.Bson;
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
    public async Task<UsersResponse> GetAllAsync()
    {   
        
        var documents = await collection.Find(new BsonDocument()).ToListAsync();

        var users = documents.Select(doc => new UserDto{
            Id = doc["_id"].AsObjectId.ToString(),
            Nick = doc["Nick"].AsString,            
            PublicKey = doc["PublicKey"].AsString,
            Image = doc["Image"].AsString,
            Posts = doc["Posts"].AsBsonArray.Select(post => new UserPostDto {
                Id = post["_id"].AsObjectId.ToString()
            }).ToArray()
        }).ToList();
            
        var response = new UsersResponse {
            Users = users
        };

        return response;
    }

    public async Task CreateAsync(CreateUserDto body)
    {
        var userExists = new FindUserByPublicKey().FindUserByPubKey(body.PublicKey);
        if (userExists != null)
        {
            throw new InvalidOperationException("This user already exists");
        }

        var user = new CreateUserDto(body.Nick, body.PublicKey, body.Image).ToBsonDocument();

        await collection.InsertOneAsync(user);
        
        return;
    }
}
