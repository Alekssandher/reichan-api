using MongoDB.Driver;
using reichan_api.src.Models.Posts;
using reichan_api.src.QueryParams;

namespace reichan_api.src.Interfaces {
    public interface IPostRepository
    {
        Task<IReadOnlyList<PostModel>> GetAllAsync(PostQueryParams queryParams);
        Task<PostModel?> GetByIdAsync(string id);
        Task<bool> UpdateVoteAsync(string id, bool vote);
        Task<bool> InsertAsync(PostModel post);
    }
}
