using MongoDB.Driver;
using reichan_api.src.Interfaces.replies;
using reichan_api.src.Models.Posts;
using reichan_api.src.Models.Replies;

namespace reichan_api.src.Repositories
{
    public class ReplyRepository : IReplyRepository
    {
        private readonly IMongoCollection<ReplyModel> _repliesCollection;
        private readonly IMongoCollection<PostModel> _postsCollection;
        public ReplyRepository(IMongoDatabase database)
        {
            _repliesCollection = database.GetCollection<ReplyModel>("replies");
            _postsCollection = database.GetCollection<PostModel>("posts");
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            IndexKeysDefinition<ReplyModel> indexKeys = Builders<ReplyModel>.IndexKeys
                .Ascending(post => post.PublicId)
                .Ascending(post => post.Author)
                .Ascending(post => post.Category)
                .Descending(post => post.CreatedAt);

            var indexModel = new CreateIndexModel<ReplyModel>(indexKeys, new CreateIndexOptions { Unique = false });
            _repliesCollection.Indexes.CreateOne(indexModel);
        }

        public async Task<IReadOnlyList<ReplyModel>> GetAllAsync()
        {
            return await _repliesCollection.Find(_ => true).ToListAsync();
        }

        public async Task<bool> InsertAsync(ReplyModel reply)
        {
            var postExists = await _postsCollection.Find(Builders<PostModel>.Filter.Eq("PublicId", reply.RepliesTo)).FirstOrDefaultAsync();

            if(postExists == null) return false;
            
            await _repliesCollection.InsertOneAsync(reply);
            return true;
        }

        
    }
}