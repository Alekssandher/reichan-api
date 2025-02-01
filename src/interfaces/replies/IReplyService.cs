public interface IReplyService {
    Task<RepliesResponse> GetAllAsync(ReplyQueryParams queryParams);
    
    Task<ReplyDto> GetByIdAsync(string id);
    Task CreateToReplyAsync(CreateReplyDto reply, string targetId);
    Task CreateAsync(CreateReplyDto replyDto, string targetId);
    //Task CreateSignedAsync(CreateSignedReplyDto replyDto);
}