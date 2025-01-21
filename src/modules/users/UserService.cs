using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;

public class UserService : IUserService
{
    private readonly MongoClient client;
    private readonly IMongoCollection<BsonDocument> collection;

    public UserService()
    {
        client = Database.Instance.GetClient();
        collection = client.GetDatabase("test").GetCollection<BsonDocument>("users");
    }
    public async Task<UsersResponse> GetAllAsync()
    {   
        
        var documents = await collection.Find(new BsonDocument()).ToListAsync();

        var users = documents.Select(doc => new UserDto{
            Id = doc["_id"].AsObjectId.ToString(),
            Nick = doc["nick"].AsString,            
            PublicKey = doc["publicKey"].AsString,
            Image = doc["image"].AsString,
            Posts = doc["posts"].AsBsonArray.Select(post => new UserPostDto {
                Id = post["_id"].AsObjectId.ToString(),
                Signature = post["signature"].AsString
            }).ToArray()
        }).ToList();
            
        var response = new UsersResponse {
            Users = users
        };

        return response;
    }

    public async Task CreateAsync(CreateUserDto body)
    {

        var  user = new BsonDocument {
            { "nick",  body.Nick },
            { "publicKey", body.PublicKey},
            { "image", body.Image },
            { "posts", new BsonArray() }
        };

        await collection.InsertOneAsync(user);
        
        return;
    }
}
