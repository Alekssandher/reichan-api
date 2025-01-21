using MongoDB.Bson;
using MongoDB.Driver;

public class PostsService : IPostService
{
    private readonly MongoClient client;
    private readonly IMongoCollection<BsonDocument> collection;
  
    public PostsService()
    {
        client = Database.Instance.GetClient();
        collection = client.GetDatabase("test").GetCollection<BsonDocument>("posts");
    }

    public async Task<PostsResponse> GetAllAsync()
    {
        var posts = await collection.Find(post => true).ToListAsync();
        return new PostsResponse();
    }

    public async Task CreateAsync(CreatePostDto post)
    {
        var newPost = new CreatePostDto (post.Title, post.Text, post.Image, post.Category, post.Author).ToBsonDocument();
        
        await collection.InsertOneAsync(newPost);
    }
}