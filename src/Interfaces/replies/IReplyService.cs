using reichan_api.src.DTOs.Replies;
using reichan_api.src.QueryParams;

namespace reichan_api.src.Interfaces.replies
{
    public interface IReplyService
    {
        Task<IReadOnlyList<ReplyResponseDto>> GetAllAsync(ReplyQueryParams replyQuery);
        //Task<PostResponseDTO?> GetByIdAsync( string id );
        Task<bool> CreateAsync( ReplyDto replyDto);
    }
}