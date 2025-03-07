using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using reichan_api.src.Utils;

namespace reichan_api.src.Models.Replies
{
    public class ReplyModel
    {

        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)] 
        public string? Id { get; init; }

        public string PublicId { get; init; } = SnowflakeIdGenerator.GenerateId().ToString();
        public required string RepliesTo { get; init; }
        public required string Content { get; init; }
        public required string Media { get; init; }
        public required string Category { get; init; }
        public required string Author { get; init; }
        public DateTime CreatedAt { get; init; }

    }
}