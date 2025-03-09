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
        public required ParentTypes ParentType { get; init; }

        [BsonRepresentation(BsonType.String)]
        public required BoardTypes BoardType { get; init; }
        public required string Content { get; init; }
        public string? Media { get; init; }
        public required string Author { get; init; }
        public DateTime CreatedAt { get; init; }

    }
}