using MongoDB.Driver;
using reichan_api.src.DTOs.Posts;
using reichan_api.src.Models.Posts;
using reichan_api.src.QueryParams;

namespace reichan_api.src.Interfaces {
    public interface IPostService 
    {
        Task<IReadOnlyList<PostResponseDTO>> GetAllAsync( PostQueryParams queryParams );
        Task<PostResponseDTO?> GetByIdAsync( string id );
        Task<bool> VoteAsync( string id, bool vote );
        Task<bool> CreateAsync( PostDto postDto );
        // Task CreateSignedAsync(CreateSignedPostDto postDto);
    }
}