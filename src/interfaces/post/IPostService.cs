public interface IPostService {
    Task<PostResponse> GetAllAsync(PostQueryParams queryParams);
    Task<PostDto> GetByIdAsync(string id);
    Task CreateAsync(CreatePostDto postDto);
    Task CreateSignedAsync(CreateSignedPostDto postDto);
}