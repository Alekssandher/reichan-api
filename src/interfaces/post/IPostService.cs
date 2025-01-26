public interface IPostService {
    Task<PostResponse> GetAllAsync();
    Task CreateAsync(CreatePostDto postDto);
    Task CreateSignedAsync(CreateSignedPostDto postDto);
}