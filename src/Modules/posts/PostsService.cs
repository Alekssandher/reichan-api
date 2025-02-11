using MongoDB.Bson;
using MongoDB.Driver;
using reichan_api.src.Models.Posts;
using reichan_api.src.Interfaces;
using reichan_api.src.Mappers;
using reichan_api.src.DTOs.Posts;

namespace reichan_api.src.Modules.Posts {
    public class PostsService : IPostService 
    {
       
        private readonly IMongoCollection<PostModel> _postsCollection;

        public PostsService(IMongoDatabase database)
        {
            _postsCollection = database.GetCollection<PostModel>("posts");
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            IndexKeysDefinition<PostModel> indexKeys = Builders<PostModel>.IndexKeys
                .Ascending(post => post.AuthorPubKey)
                .Ascending(post => post.Author)
                .Ascending(post => post.Category)
                .Descending(post => post.CreatedAt); 
            
            CreateIndexModel<PostModel> indexModel = new CreateIndexModel<PostModel>(indexKeys, new CreateIndexOptions { Unique = false });

            _postsCollection.Indexes.CreateOne(indexModel);
        }

        public async Task<IReadOnlyList<PostResponseDTO>> GetAllAsync( FilterDefinition<PostModel> filter, FindOptions<PostModel> options )
        {            
            ProjectionDefinition<PostModel, PostResponseDTO> projection = Builders<PostModel>.Projection.Expression(post => new PostResponseDTO
            {
                Id = post.Id!,
                Title = post.Title,
                Content = post.Content,
                Author = post.Author,
                Media = post.Media,
                Category = post.Category,
                CreatedAt = post.CreatedAt
            });

            IReadOnlyList<PostResponseDTO> posts = await _postsCollection
                .Find(filter)
                .Sort(options.Sort)
                .Skip(options.Skip)
                .Limit(options.Limit)
                .Project(projection) 
                .ToListAsync();

            return posts;
           
        }

        public async Task<PostResponseDTO?> GetByIdAsync( string id ) {

            FilterDefinition<PostModel> filter = Builders<PostModel>.Filter.Eq("_id", ObjectId.Parse(id));

            PostModel post = await _postsCollection.Find(filter).FirstOrDefaultAsync();

            PostResponseDTO postDto = post.ResponseToDto();
            
            return postDto;
        }

        public async Task<bool> VoteAsync ( string id, bool vote ) {

            string kindVote = vote ? "UpVotes" : "DownVotes";

            UpdateDefinition<PostModel> update = Builders<PostModel>.Update.Inc(kindVote, 1);

            UpdateResult? result = await _postsCollection.UpdateOneAsync(
                Builders<PostModel>.Filter.Eq("_id", ObjectId.Parse(id)), 
                update
            );

            return result.ModifiedCount > 0;
            
        }

        public async Task<bool> CreateAsync( PostDto postDto ) {
            
            await _postsCollection.InsertOneAsync(postDto.ToModel());

            return true;
        }
    }
}