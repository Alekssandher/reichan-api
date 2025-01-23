using MongoDB.Bson;
using MongoDB.Driver;

public class PostsService : IPostService
{
    private readonly MongoClient client;
    private readonly IMongoCollection<BsonDocument> postsCollection;
    private readonly IMongoCollection<BsonDocument> usersCollection;
    public PostsService()
    {
        client = Database.Instance.GetClient();
        postsCollection = client.GetDatabase("test").GetCollection<BsonDocument>("posts");
        usersCollection = client.GetDatabase("test").GetCollection<BsonDocument>("users");
    }

    public async Task<PostsResponse> GetAllAsync()
    {
        var posts = await postsCollection.Find(post => true).ToListAsync();
        return new PostsResponse();
    }

    public async Task CreateSignedAsync(CreateSignedPostDto post)
    {
        var postId = ObjectId.GenerateNewId();

        var newPost = new CreateSignedPostDto(post.PublicKey, post.Title, post.Text, post.Image, post.Category, post.Author, post.Signature)
            .ToBsonDocument();
        newPost["_id"] = postId;  

        var newUserPost = new BsonDocument
        {
            { "_id", newPost["_id"] }
        };

        await usersCollection.UpdateOneAsync(
            Builders<BsonDocument>.Filter.Eq("publicKey", post.PublicKey),
            Builders<BsonDocument>.Update.Push("posts", newUserPost)
        );

        // Inserindo o post na coleção de posts
        await postsCollection.InsertOneAsync(newPost);

        return;
    }
}