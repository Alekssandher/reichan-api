using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

public class PostsService : IPostService
{
    private readonly MongoClient client;
    private readonly IMongoCollection<BsonDocument> postsCollection;

    public PostsService(DatabaseConfig config)
    {
        client = Database.Instance.GetClient();
        postsCollection = client.GetDatabase(config.DatabaseName).GetCollection<BsonDocument>(config.PostsCollection);
    }

    public async Task<PostResponse> GetAllAsync()
    {
        List<BsonDocument> posts = await postsCollection.Find(post => true).ToListAsync();
        List<PostDto> postsDto = posts.Select(post => BsonSerializer.Deserialize<PostDto>(post)).ToList();
        
        PostResponse response = new PostResponse {
            Posts = postsDto
        };

        return response;
    }

    public async Task CreateAsync(CreatePostDto postDto)
    {
        
        await postsCollection.InsertOneAsync(postDto.ToBsonDocument());

        return;
    }
    public async Task CreateSignedAsync(CreateSignedPostDto post)
    {
        UserDto? userExists = new FindUserByPublicKey().FindUserByPubKey(post.PublicKey);

        if (userExists == null)
        {
            throw new InvalidOperationException("User not found, is the PublicKey correct?");
        }

        BsonDocument newPost = post.ToBsonDocument();

        await postsCollection.InsertOneAsync(newPost);

        return;
    }
}