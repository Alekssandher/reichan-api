using MongoDB.Bson;

public class CreateReplyDto
{
    public ObjectId Id { get; set; }
    public string PostId { get; set; } = "";
    public string? Author { get; set; }
    public string Text { get; set; }
    public IReadOnlyCollection<ReplyDto> Replies { get; init; }  
    public string? Image { get; set; } = null;
    public string? Category { get; set; } = null;
    public DateTime CreatedAt { get; set; }

    public CreateReplyDto(string author, string text)
    {
        Id = ObjectId.GenerateNewId();
        Author = author;
        Text = text;
        Replies = new List<ReplyDto>().AsReadOnly();
        CreatedAt = DateTime.UtcNow;
    }
}