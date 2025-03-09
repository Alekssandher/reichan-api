
using reichan_api.src.DTOs.Replies;
using reichan_api.src.Enums;
using reichan_api.src.Interfaces.replies;
using reichan_api.src.Mappers;
using reichan_api.src.Models.Posts;
using reichan_api.src.Models.Replies;
using reichan_api.src.QueryParams;

namespace reichan_api.src.Modules.replies
{
    public class RepliesService : IReplyService
    {
        private readonly IReplyRepository _repository;

        public RepliesService(IReplyRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<ReplyResponseDto>> GetAllAsync(BoardTypes boardTypes,ReplyQueryParams replyQuery)
        {
            IReadOnlyList<ReplyModel> replies = await _repository.GetAllAsync(boardTypes, replyQuery);
            return replies.Select(reply => reply.ToResponseDto()).ToList();
        }

        public async Task<bool> CreateAsync(ReplyDto replyDto)
        {
            return await _repository.InsertAsync(replyDto.ToModel());
        }

        
    }
}