public class PostResponse
{
    public List<PostDto> Posts { get; set; }

    public PostResponse()
    {
        Posts = new List<PostDto>();
    }
}