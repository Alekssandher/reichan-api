using MongoDB.Bson.Serialization.Attributes;

public class ReplyDto {
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }
    public string PostId { get; set; } 
    public string Author { get; set; }
    public string Text { get; set; }
    public List<ReplyDto> Replies { get; set; }
    public string? Image { get; set; } = null;
    public string Date { get; set; }

}