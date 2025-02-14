using MongoDB.Driver;
using reichan_api.src.Interfaces;
using reichan_api.src.Models.Posts;
using reichan_api.src.QueryParams;

namespace reichan_api.src.Repositories {
    public class PostsRepository : IPostRepository
    {
        private readonly IMongoCollection<PostModel> _postsCollection;

        public PostsRepository(IMongoDatabase database)
        {
            _postsCollection = database.GetCollection<PostModel>("posts");
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            IndexKeysDefinition<PostModel> indexKeys = Builders<PostModel>.IndexKeys
                .Ascending(post => post.PublicId)
                .Ascending(post => post.Author)
                .Ascending(post => post.Category)
                .Descending(post => post.CreatedAt);

            var indexModel = new CreateIndexModel<PostModel>(indexKeys, new CreateIndexOptions { Unique = false });
            _postsCollection.Indexes.CreateOne(indexModel);
        }

        public async Task<IReadOnlyList<PostModel>> GetAllAsync(PostQueryParams queryParams)
        {
            FilterDefinition<PostModel> filter = queryParams.GetFilter();
            FindOptions<PostModel> options = queryParams.GetFindOptions();
            
            return await _postsCollection.Find(filter).Sort(options.Sort).Skip(options.Skip).Limit(options.Limit).ToListAsync();
        }

        public async Task<PostModel?> GetByIdAsync(string id)
        {
            return await _postsCollection.Find(Builders<PostModel>.Filter.Eq("PublicId", id)).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateVoteAsync(string id, bool vote)
        {
            string kindVote = vote ? "UpVotes" : "DownVotes";
            UpdateDefinition<PostModel> update = Builders<PostModel>.Update.Inc(kindVote, 1);
            UpdateResult? result = await _postsCollection.UpdateOneAsync(Builders<PostModel>.Filter.Eq("PublicId", id), update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> InsertAsync(PostModel post)
        {
            await _postsCollection.InsertOneAsync(post);
            return true;
        }
    }
}
