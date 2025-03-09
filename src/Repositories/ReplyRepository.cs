using MongoDB.Driver;
using reichan_api.src.Enums;
using reichan_api.src.Interfaces.replies;
using reichan_api.src.Models.Posts;
using reichan_api.src.Models.Replies;
using reichan_api.src.QueryParams;

namespace reichan_api.src.Repositories
{
    public class ReplyRepository : IReplyRepository
    {
        private readonly IMongoCollection<ReplyModel> _repliesCollection;
        private readonly IMongoCollection<ThreadModel> _threadsCollection;
        private IMongoDatabase _database;
        public ReplyRepository(IMongoDatabase database)
        {
            _repliesCollection = database.GetCollection<ReplyModel>("replies");
            _threadsCollection = database.GetCollection<ThreadModel>("threads");
            _database = database;
            
        }

        public async Task<IReadOnlyList<ReplyModel>> GetAllAsync(BoardTypes boardType, ReplyQueryParams replyQuery)
        {
            FilterDefinition<ReplyModel> filter = replyQuery.GetFilter();
            FindOptions<ReplyModel> options = replyQuery.GetFindOptions();

            return await GetCollection(boardType.ToString()).Find(filter).Sort(options.Sort).Skip(options.Skip).Limit(options.Limit).ToListAsync();
        }

        public async Task<bool> InsertAsync(ReplyModel reply)
        {
            BoardTypes boardType = reply.BoardType;
            ParentTypes parentType = reply.ParentType;
            var exists = await CheckIfTargetExists(reply.ParentId, boardType, parentType);

            if(exists.Item1 == null && exists.Item2 == null) return false;
            
            await GetCollection(reply.BoardType.ToString()).InsertOneAsync(reply);
            return true;
        }

        private async Task<(ThreadModel?, ReplyModel?)> CheckIfTargetExists(string parentId, BoardTypes boardType, ParentTypes parentType)
        {
            if (parentType == ParentTypes.thread)
            {
                var filter = Builders<ThreadModel>.Filter.And(
                    Builders<ThreadModel>.Filter.Eq(t => t.PublicId, parentId),
                    Builders<ThreadModel>.Filter.Eq(t => t.BoardType, boardType)
                );
                return (await _threadsCollection.Find(filter).FirstOrDefaultAsync(), null);
            }
            else if (parentType == ParentTypes.reply)
            {
                var filter = Builders<ReplyModel>.Filter.And(
                    Builders<ReplyModel>.Filter.Eq(r => r.PublicId, parentId),
                    Builders<ReplyModel>.Filter.Eq(r => r.BoardType, boardType)
                );
                var result = await GetCollection(boardType.ToString()).Find(filter).FirstOrDefaultAsync();
                return (null, result);
            }

            throw new ArgumentException($"Invalid ParentType: {parentType}");
        }

        private IMongoCollection<ReplyModel> GetCollection(string parentType) =>
            _database.GetCollection<ReplyModel>(parentType);
    }
    
}