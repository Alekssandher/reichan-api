using MongoDB.Driver;
using ReichanApi.Models;
using ReichanApi.QueryParams;

namespace ReichanApi.Interfaces {
    public interface IPostService 
    {
        Task<IReadOnlyList<PostModel>> GetAllAsync( FilterDefinition<PostModel> filter, FindOptions<PostModel> options );
        Task<PostModel?> GetByIdAsync( string id );
        // Task VotePostAsync(int kindVote, string id);
        // Task CreateAsync(CreatePostDto postDto);
        // Task CreateSignedAsync(CreateSignedPostDto postDto);
    }
}