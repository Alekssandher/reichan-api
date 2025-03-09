using reichan_api.src.DTOs.Threads;
using reichan_api.src.QueryParams;

namespace reichan_api.src.Interfaces {
    public interface IThreadService 
    {
        Task<IReadOnlyList<ThreadResponseDto>> GetAllAsync( ThreadQueryParams queryParams );
        Task<ThreadResponseDto?> GetByIdAsync( string id );
        Task<bool> VoteAsync( string id, bool vote );
        Task<bool> CreateAsync( ThreadDto postDto );
        // Task CreateSignedAsync(CreateSignedPostDto postDto);
    }
}