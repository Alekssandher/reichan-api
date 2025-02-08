namespace ReichanApi.DTOs
{
    public class PostResponseDTO {
        public required string Id { get; set; }
        public string? AuthorPubKey { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string Image { get; set; }
        public required string Category { get; set; }
        public required string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Signature { get; set; }
        public bool Active { get; set; }
        public int Upvotes { get; set; }
        public int DownVotes { get; set; }
    }
}