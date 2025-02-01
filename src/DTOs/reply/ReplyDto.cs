using MongoDB.Bson.Serialization.Attributes;

public class ReplyDto {
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public required string Id { get; set; }
    public required string RepliesTo { get; set; } 
    public required string Author { get; set; }
    public required string Text { get; set; }
    public required List<ReplyDto> Replies { get; set; }
    public string? Image { get; set; } = null;
    public string? Category { get; set; } = null;
    public DateTime CreatedAt { get; set; }

}