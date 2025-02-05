using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class UserDto
{
    [BsonId] 
    [BsonRepresentation(BsonType.ObjectId)] 
    public required string Id { get; set; }
    public required string Nick { get; set; }
    public required string PublicKey { get; set; } 
    public required string Image { get; set; } 
    public required List<string> Posts { get; set; }
    public required DateTime CreatedAt { get; set; }
}