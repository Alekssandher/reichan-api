using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class PostDto {

    [BsonId] 
    [BsonRepresentation(BsonType.ObjectId)] 
    public required string Id { get; set; }
    
    public string? PublicKey { get; set; }
    public required string Title { get; set; }
    public required string Text { get; set; }
    public required string Image { get; set; }
    public required string Category { get; set; }
    public required string Author { get; set; }
    public required List<string> Replies { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Signature { get; set; }
    public bool Active { get; set; }
    public int Votes { get; set; }
}