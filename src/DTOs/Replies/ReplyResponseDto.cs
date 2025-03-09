using reichan_api.src.Enums;

namespace reichan_api.src.DTOs.Replies
{
    public class ReplyResponseDto
    {
        public required string Id { get; init; }
        public required string ParentId { get; init; }
        public required ParentTypes ParentType { get; init; }
        public required BoardTypes BoardType { get; init; }
        public required string Content { get; init; }
        public string? Media { get; init; }
        public required string Author { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}