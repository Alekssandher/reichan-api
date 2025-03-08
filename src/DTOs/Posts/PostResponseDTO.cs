using reichan_api.src.Enums;

namespace reichan_api.src.DTOs.Posts
{
    public class PostResponseDTO {
        public required string Id { get; init; }
        public required string Title { get; init; }
        public required string Content { get; init; }
        public required string Media { get; init; }
        public required Categories Category { get; init; }
        public required string Author { get; init; }
        public DateTime CreatedAt { get; init; }
        public bool Active { get; init; }
        public int UpVotes { get; init; }
        public int DownVotes { get; init; }
    }
}