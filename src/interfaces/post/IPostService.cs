public interface IPostService {
    Task<PostsResponse> GetAllAsync();
    Task CreateSignedAsync(CreateSignedPostDto postDto);
}