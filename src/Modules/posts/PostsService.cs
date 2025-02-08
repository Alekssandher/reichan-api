using MongoDB.Bson;
using MongoDB.Driver;
using ReichanApi.Interfaces;
using ReichanApi.Models;
using ReichanApi.QueryParams;

namespace ReichanApi.Services {
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

        public async Task<PostModel?> GetByIdAsync( string id ) {

            FilterDefinition<PostModel> filter = Builders<PostModel>.Filter.Eq("_id", ObjectId.Parse(id));

            PostModel post = await _postsCollection.Find(filter).FirstOrDefaultAsync();

            return post;
        }
    }
}