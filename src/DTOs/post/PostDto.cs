using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class PostDto {

    [BsonId] 
    [BsonRepresentation(BsonType.ObjectId)] 
    public string Id { get; set; }
    
    public string? PublicKey { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public string Image { get; set; }
    public string Category { get; set; }
    public string Author { get; set; }
    public List<string> Replies { get; set; }
    public string Date { get; set; }
    public string? Signature { get; set; }
    public bool Active { get; set; }
    public int Votes { get; set; }
}