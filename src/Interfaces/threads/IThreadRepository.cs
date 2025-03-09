using MongoDB.Driver;
using reichan_api.src.Models.Threads;
using reichan_api.src.QueryParams;

namespace reichan_api.src.Interfaces {
    public interface IThreadRepository
    {
        Task<IReadOnlyList<ThreadModel>> GetAllAsync(ThreadQueryParams queryParams);
        Task<ThreadModel?> GetByIdAsync(string id);
        Task<bool> UpdateVoteAsync(string id, bool vote);
        Task<bool> InsertAsync(ThreadModel post);
    }
}
