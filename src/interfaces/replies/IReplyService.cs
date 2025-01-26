public interface IReplyService {
    //Task<RepliesResponse> GetAllAsync();
    Task CreateAsync(CreateReplyDto replyDto, string targetId);
    //Task CreateSignedAsync(CreateSignedReplyDto replyDto);
}