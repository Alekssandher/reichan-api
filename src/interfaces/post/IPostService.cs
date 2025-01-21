public interface IPostService {
    Task<PostsResponse> GetAllAsync();
    Task CreateAsync(CreatePostDto postDto);
}