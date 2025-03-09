using MongoDB.Driver;
using reichan_api.src.Models.Posts;
using reichan_api.src.QueryParams;

namespace reichan_api.src.Interfaces {
    public interface IPostRepository
    {
        Task<IReadOnlyList<ThreadModel>> GetAllAsync(PostQueryParams queryParams);
        Task<ThreadModel?> GetByIdAsync(string id);
        Task<bool> UpdateVoteAsync(string id, bool vote);
        Task<bool> InsertAsync(ThreadModel post);
    }
}
