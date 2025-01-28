using MongoDB.Bson;

public class CreateReplyDto
{
    public ObjectId Id { get; set; }
    public string PostId { get; set; } = "";
    public string? Author { get; set; }
    public string Text { get; init; }
    public IReadOnlyCollection<ReplyDto> Replies { get; init; }  
    public string? Image { get; init; } = null;
    public string? Category { get; init; } = null;
    public DateTime CreatedAt { get; init; }

    public CreateReplyDto(string author, string text)
    {
        Id = ObjectId.GenerateNewId();
        Author = author;
        Text = text;
        Replies = new List<ReplyDto>().AsReadOnly();
        CreatedAt = DateTime.UtcNow;
    }
}