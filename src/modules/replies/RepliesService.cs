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

    public async Task<RepliesResponse> GetAllAsync(ReplyQueryParams queryParams)
    {
        FilterDefinition<BsonDocument> filter = queryParams.GetFilter();

        List<BsonDocument> replies = await repliesCollection.Find(filter).Skip(queryParams.Skip).Limit(queryParams.Limit).ToListAsync();
        List<ReplyDto> repliesDto = replies.Select(reply => BsonSerializer.Deserialize<ReplyDto>(reply)).ToList();
        
        RepliesResponse response = new RepliesResponse {
            Replies = repliesDto
        };

        return response;
    }

    public async Task<ReplyDto> GetByIdAsync(string id)
    {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));

        BsonDocument reply = await repliesCollection.Find(filter).FirstOrDefaultAsync();

        if (reply == null)
        {
            throw new InvalidOperationException("Reply not found.");
        }

        ReplyDto replyDto = BsonSerializer.Deserialize<ReplyDto>(reply);

        return replyDto;
    }

    public async Task CreateAsync(CreateReplyDto replyDto, string targetId)
    {

        Console.WriteLine(targetId);
        var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(targetId));
        
        BsonDocument target = await postsCollection.Find(filter).FirstOrDefaultAsync();

        if(target == null)
        {
            throw new InvalidOperationException("Post not found");
        }

        BsonDocument newReply = replyDto.ToBsonDocument();

        var update = Builders<BsonDocument>.Update.Push("Replies", replyDto.Id.ToString());

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