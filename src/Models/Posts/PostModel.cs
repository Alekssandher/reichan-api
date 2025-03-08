using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using reichan_api.src.Enums;
using reichan_api.src.Utils;

namespace reichan_api.src.Models.Posts {
    public class PostModel 
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)] 
        public string? Id { get; init; }
        public required string PublicId { get; init; }
        public string? AuthorPubKey { get; init; }
        public required string Title { get; init; }
        public required string Content { get; init; }
        public required string Media { get; init; }

        [BsonRepresentation(BsonType.String)]
        public required Categories Category { get; init; }
        public required string Author { get; init; }
        public DateTime CreatedAt { get; init; }
        public string? Signature { get; init; }
        public bool Active { get; init; } = true;
        public int UpVotes { get; init; }
        public int DownVotes { get; init; }
    }
}
