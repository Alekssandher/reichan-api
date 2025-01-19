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

        var users = documents.Select(doc => new UserDto {
            Id = doc["_id"].AsObjectId.ToString(),
            Name = doc["name"].AsString,            
            Email = doc["email"].AsString,
            Password = doc["password"].AsString
        }).ToList();
            
        var response = new UsersResponse {
            Users = users
        };

        return response;
    }

    public async Task CreateAsync(CreateUserDto body)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("email", body.Email);

        var userExists = collection.Find(filter).FirstOrDefault();

        if(userExists != null) {
            throw new InvalidOperationException("Email already exists.");
        }

        var  user = new BsonDocument {
            { "name",  body.Name },
            { "email", body.Email},
            { "password", body.Password}
        };
        await collection.InsertOneAsync(user);
        
        return;
    }
}
