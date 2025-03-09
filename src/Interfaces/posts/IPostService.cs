using reichan_api.src.DTOs.Posts;
using reichan_api.src.QueryParams;

namespace reichan_api.src.Interfaces {
    public interface IPostService 
    {
        Task<IReadOnlyList<ThreadResponseDto>> GetAllAsync( PostQueryParams queryParams );
        Task<ThreadResponseDto?> GetByIdAsync( string id );
        Task<bool> VoteAsync( string id, bool vote );
        Task<bool> CreateAsync( ThreadDto postDto );
        // Task CreateSignedAsync(CreateSignedPostDto postDto);
    }
}