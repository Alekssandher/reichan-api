public interface IReplyService {
    Task<RepliesResponse> GetAllAsync(ReplyQueryParams queryParams);
    Task<ReplyDto> GetByIdAsync(string id);
    Task CreateAsync(CreateReplyDto replyDto, string targetId);
    //Task CreateSignedAsync(CreateSignedReplyDto replyDto);
}