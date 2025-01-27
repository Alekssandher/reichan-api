
using MongoDB.Bson;

public class CreateReplyDto {
    public ObjectId Id { get; set; }
    public string PostId { get; set; } = "";
    public string Author { get; set; }
    public string Text { get; set; }
    public List<ReplyDto> Replies { get; set; }
    public string? Image { get; set; } = null;
    public string Date { get; set; }
    public CreateReplyDto(string author, string text)
    {
        Id = ObjectId.GenerateNewId();
        Author = author;
        Text = text;
        Replies = new List<ReplyDto>();
        Date = DateTime.UtcNow.ToString("o");
    }
}