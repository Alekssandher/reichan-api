using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using reichan_api.Utils;

namespace reichan_api.src.Models.Posts {
    public class PostModel 
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)] 
        public string? Id { get; init; }
        public string PublicId { get; init; } = SnowflakeIdGenerator.GenerateId().ToString();
        public string? AuthorPubKey { get; init; }
        public required string Title { get; init; }
        public required string Content { get; init; }
        public required string Media { get; init; }
        public required string Category { get; init; }
        public required string Author { get; init; }
        public DateTime CreatedAt { get; init; }
        public string? Signature { get; init; }
        public bool Active { get; init; } = true;
        public int UpVotes { get; init; }
        public int DownVotes { get; init; }
    }
}
//new IdGenerator(0).CreateId()