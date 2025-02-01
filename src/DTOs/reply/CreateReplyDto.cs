using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

public class CreateReplyDto
{
    public ObjectId Id { get; set; }
    public string RepliesTo { get; set; } = "";
    public string? Author { get; set; }

    [Required(ErrorMessage = "Text is required.")]
    [StringLength(600, MinimumLength = 1, ErrorMessage = "Text chars must be between 1 - 600 chars")]
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