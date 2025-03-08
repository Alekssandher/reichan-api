using MongoDB.Driver;
using reichan_api.src.Interfaces.replies;
using reichan_api.src.Models.Posts;
using reichan_api.src.Models.Replies;
using reichan_api.src.QueryParams;

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
                .Descending(post => post.CreatedAt);

            var indexModel = new CreateIndexModel<ReplyModel>(indexKeys, new CreateIndexOptions { Unique = false });
            _repliesCollection.Indexes.CreateOne(indexModel);
        }

        public async Task<IReadOnlyList<ReplyModel>> GetAllAsync(ReplyQueryParams replyQuery)
        {
            FilterDefinition<ReplyModel> filter = replyQuery.GetFilter();
            FindOptions<ReplyModel> options = replyQuery.GetFindOptions();

            return await _repliesCollection.Find(filter).Sort(options.Sort).Skip(options.Skip).Limit(options.Limit).ToListAsync();
        }

        public async Task<bool> InsertAsync(ReplyModel reply)
        {
            var exists = CheckIfTargetExists(reply.ParentId, "post");

            if(exists == null) return false;
            
            await _repliesCollection.InsertOneAsync(reply);
            return true;
        }

        private async Task<(PostModel?, ReplyModel?)> CheckIfTargetExists(string parentId, string parentType)
        {
            if(parentType == "post") return (await _postsCollection.Find(Builders<PostModel>.Filter.Eq("PublicId", parentId)).FirstOrDefaultAsync(), null);
            else if(parentType == "reply") return (null, await _repliesCollection.Find(Builders<ReplyModel>.Filter.Eq("PublicId", parentId)).FirstOrDefaultAsync());
            else throw new Exception("Invalid parentType.");
        }
    }
}