public interface IPostService {
    Task<PostResponse> GetAllAsync(PostQueryParams queryParams);
    Task CreateAsync(CreatePostDto postDto);
    Task CreateSignedAsync(CreateSignedPostDto postDto);
}