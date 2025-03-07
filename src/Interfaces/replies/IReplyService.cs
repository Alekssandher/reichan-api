using reichan_api.src.DTOs.Replies;

namespace reichan_api.src.Interfaces.replies
{
    public interface IReplyService
    {
        Task<IReadOnlyList<ReplyResponseDto>> GetAllAsync();
        //Task<PostResponseDTO?> GetByIdAsync( string id );
        Task<bool> CreateAsync( ReplyDto replyDto );
    }
}