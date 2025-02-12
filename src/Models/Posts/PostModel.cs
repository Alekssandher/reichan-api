using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace reichan_api.src.Models.Posts {
    public class PostModel 
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)] 
        public string? Id { get; set; }
        
        [BsonRepresentation(BsonType.String)] 
        public Guid PublicId { get; set; } = Guid.NewGuid();
        public string? AuthorPubKey { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string Media { get; set; }
        public required string Category { get; set; }
        public required string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Signature { get; set; }
        public bool Active { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
    }
}