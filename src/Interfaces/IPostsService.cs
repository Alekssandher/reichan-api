using MongoDB.Driver;
using reichan_api.src.DTOs.Posts;
using reichan_api.src.Models.Posts;

namespace reichan_api.src.Interfaces {
    public interface IPostService 
    {
        Task<IReadOnlyList<PostModel>> GetAllAsync( FilterDefinition<PostModel> filter, FindOptions<PostModel> options );
        Task<PostResponseDTO?> GetByIdAsync( string id );
        Task<bool> VotePostAsync( string id, bool vote );
        //Task CreateAsync(PostDto postDto);
        // Task CreateSignedAsync(CreateSignedPostDto postDto);
    }
}