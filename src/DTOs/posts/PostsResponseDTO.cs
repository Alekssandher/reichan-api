using ReichanApi.Models;

namespace ReichanApi.DTOs {
    public class PostsResponseDTO
    {
        public required byte Status { get; set; }
        public required IReadOnlyList<PostModel> Data { get; set; }
    }

}