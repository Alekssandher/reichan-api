using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using reichan_api.src.Enums;
using reichan_api.src.Utils;

namespace reichan_api.src.Models.Replies
{
    public class ReplyModel
    {

        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)] 
        public string? Id { get; init; }
        public required string PublicId { get; init; } 
        public required string ParentId { get; init; }

        [BsonRepresentation(BsonType.String)]
        public required ParentType ParentType { get; init; }
        public required string Content { get; init; }
        public required string Media { get; init; }
        public required string Author { get; init; }
        public DateTime CreatedAt { get; init; }

    }
}