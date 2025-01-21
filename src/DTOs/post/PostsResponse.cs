public class PostsResponse {
    public List<PostDto> Users { get; set; }

    public PostsResponse() {
        Users = new List<PostDto>();
    }
}