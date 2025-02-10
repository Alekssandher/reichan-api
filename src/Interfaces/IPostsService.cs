using MongoDB.Driver;
using reichan_api.src.DTOs.Posts;
using reichan_api.src.Models.Posts;

namespace reichan_api.src.Interfaces {
    public interface IPostService 
    {
        Task<IReadOnlyList<PostResponseDTO>> GetAllAsync( FilterDefinition<PostModel> filter, FindOptions<PostModel> options );
        Task<PostResponseDTO?> GetByIdAsync( string id );
        Task<bool> VoteAsync( string id, bool vote );
        Task<bool> CreateAsync( PostDto postDto );
        // Task CreateSignedAsync(CreateSignedPostDto postDto);
    }
}