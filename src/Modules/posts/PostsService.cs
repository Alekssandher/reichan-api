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

        public PostsService(IMongoCollection<PostModel> postsCollection)
        {
            _postsCollection = postsCollection;
           
        }

        public async Task<IReadOnlyList<PostModel>> GetAllAsync( FilterDefinition<PostModel> filter, FindOptions<PostModel> options )
        {            
            IReadOnlyList<PostModel> posts = await _postsCollection.Find(filter).Sort(options.Sort).Skip(options.Skip).Limit(options.Limit).ToListAsync();
            
            return posts;
        }

        public async Task<PostResponseDTO?> GetByIdAsync( string id ) {

            if (string.IsNullOrWhiteSpace(id) || !ObjectId.TryParse(id, out ObjectId objectId))
            {
                return null;
            }
            
            FilterDefinition<PostModel> filter = Builders<PostModel>.Filter.Eq("_id", objectId);

            PostModel post = await _postsCollection.Find(filter).FirstOrDefaultAsync();

            PostResponseDTO postDto = post.ResponseToDto();
            
            return postDto;
        }

        public async Task<bool> VoteAsync ( string id, bool vote ) {
            if ( string.IsNullOrWhiteSpace(id) || !ObjectId.TryParse(id, out ObjectId objectId))
            {
                return false;
            }

            FilterDefinition<PostModel> filter = Builders<PostModel>.Filter.Eq("_id", ObjectId.Parse(id));

            string kindVote = vote ? "UpVotes" : "DownVotes";

            UpdateDefinition<PostModel> update = Builders<PostModel>.Update.Inc(kindVote, 1);

            UpdateResult? result = await _postsCollection.UpdateOneAsync(filter, update);

            if (result.ModifiedCount == 0) return false; 
            else return true;
            
        }

        public async Task<bool> CreateAsync( PostDto postDto ) {
            
            await _postsCollection.InsertOneAsync(postDto.ToModel());

            return true;
        }
    }
}