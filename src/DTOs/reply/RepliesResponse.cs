public class RepliesResponse {
    public List<ReplyDto> Replies { get; set; }

    public RepliesResponse()
    {
        Replies = new List<ReplyDto>();
    }
}