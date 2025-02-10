using reichan_api.src.Models.Posts;

namespace ReichanApi.DTOs {
    public class PostsResponseDTO
    {
        public required byte Status { get; set; }
        public required IReadOnlyList<PostModel> Data { get; set; }
    }

}