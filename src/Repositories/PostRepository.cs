using MongoDB.Driver;
using reichan_api.src.Interfaces;
using reichan_api.src.Models.Posts;
using reichan_api.src.QueryParams;

namespace reichan_api.src.Repositories {
    public class PostsRepository : IPostRepository
    {
        private readonly IMongoCollection<ThreadModel> _threadsCollection;

        public PostsRepository(IMongoDatabase database)
        {
            _threadsCollection = database.GetCollection<ThreadModel>("threads");
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            IndexKeysDefinition<ThreadModel> indexKeys = Builders<ThreadModel>.IndexKeys
                .Ascending(post => post.PublicId)
                .Ascending(post => post.Author)
                .Ascending(post => post.BoardType)
                .Descending(post => post.CreatedAt);

            var indexModel = new CreateIndexModel<ThreadModel>(indexKeys, new CreateIndexOptions { Unique = false });
            _threadsCollection.Indexes.CreateOne(indexModel);
        }

        public async Task<IReadOnlyList<ThreadModel>> GetAllAsync(PostQueryParams queryParams)
        {
            FilterDefinition<ThreadModel> filter = queryParams.GetFilter();
            FindOptions<ThreadModel> options = queryParams.GetFindOptions();
            
            return await _threadsCollection.Find(filter).Sort(options.Sort).Skip(options.Skip).Limit(options.Limit).ToListAsync();
        }

        public async Task<ThreadModel?> GetByIdAsync(string id)
        {
            return await _threadsCollection.Find(Builders<ThreadModel>.Filter.Eq("PublicId", id)).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateVoteAsync(string id, bool vote)
        {
            string kindVote = vote ? "UpVotes" : "DownVotes";
            UpdateDefinition<ThreadModel> update = Builders<ThreadModel>.Update.Inc(kindVote, 1);
            UpdateResult? result = await _threadsCollection.UpdateOneAsync(Builders<ThreadModel>.Filter.Eq("PublicId", id), update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> InsertAsync(ThreadModel post)
        {
            await _threadsCollection.InsertOneAsync(post);
            return true;
        }
    }
}
