using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

public class RepliesService : IReplyService
{
    private readonly MongoClient client;
    private readonly IMongoCollection<BsonDocument> repliesCollection; 
    private readonly IMongoCollection<BsonDocument> postsCollection;

    public RepliesService( DatabaseConfig config )
    {
        client = Database.Instance.GetClient();
        repliesCollection = client.GetDatabase(config.DatabaseName).GetCollection<BsonDocument>(config.RepliesCollection);
        postsCollection = client.GetDatabase(config.DatabaseName).GetCollection<BsonDocument>(config.PostsCollection);
    }

    // public async Task<PostResponse> GetAllAsync()
    // {
    //     List<BsonDocument> posts = await postsCollection.Find(post => true).ToListAsync();
    //     List<PostDto> postsDto = posts.Select(post => BsonSerializer.Deserialize<PostDto>(post)).ToList();
        
    //     PostResponse response = new PostResponse {
    //         Posts = postsDto
    //     };

    //     return response;
    // }

    public async Task CreateAsync(CreateReplyDto replyDto, string targetId)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(targetId));
        
        BsonDocument target = await postsCollection.Find(filter).FirstOrDefaultAsync();

        if(target == null)
        {
            throw new InvalidOperationException("Post not found");
        }

        BsonDocument newReply = replyDto.ToBsonDocument();

        var update = Builders<BsonDocument>.Update.Push("Replies", targetId);

        var result = await postsCollection.UpdateOneAsync(filter, update); 

        if (result.MatchedCount == 0)
        {
            throw new InvalidOperationException("Post not found");
        }

        await repliesCollection.InsertOneAsync(newReply);
        
        return;
    }
    // public async Task CreateSignedAsync(CreateSignedPostDto post)
    // {
    //     UserDto? userExists = new FindUserByPublicKey().FindUserByPubKey(post.PublicKey);

    //     if (userExists == null)
    //     {
    //         throw new InvalidOperationException("User not found, is the PublicKey correct?");
    //     }

    //     BsonDocument newPost = post.ToBsonDocument();

    //     await postsCollection.InsertOneAsync(newPost);

    //     return;
    // }
}