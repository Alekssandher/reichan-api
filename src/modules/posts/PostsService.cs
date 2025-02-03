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

    public async Task<PostResponse> GetAllAsync(PostQueryParams queryParams)
    {
        FilterDefinition<BsonDocument> filter = queryParams.GetFilter();
        FindOptions<BsonDocument> options = queryParams.GetFindOptions();
        
        List<BsonDocument> posts = await postsCollection.Find(filter).Sort(options.Sort).Skip(options.Skip).Limit(options.Limit).ToListAsync();
        List<PostDto> postsDto = posts.Select(post => BsonSerializer.Deserialize<PostDto>(post)).ToList();
        
        PostResponse response = new PostResponse {
            Posts = postsDto
        };

        return response;
    }
    public async Task<PostDto> GetByIdAsync(string id)
    {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));

        BsonDocument post = await postsCollection.Find(filter).FirstOrDefaultAsync();

        if (post == null) throw new InvalidOperationException("Post not found.");
        

        PostDto postDto = BsonSerializer.Deserialize<PostDto>(post);

        return postDto;
    }

    public async Task VotePostAsync(int vote, string id)
    {
        Console.WriteLine(vote);
        if (vote != 1 && vote != -1) throw new ArgumentException("Vote must be either 1 or -1.", nameof(vote));
        
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
        UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Inc("Votes", vote);

        var result = await postsCollection.UpdateOneAsync(filter, update);

        if (result.ModifiedCount == 0) throw new InvalidOperationException("Post not found or vote not applied.");
        
        return;
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