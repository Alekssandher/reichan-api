public class ReplyDto {
    public string Author { get; set; }
    public string Text { get; set; }
    public List<ReplyDto> Replies { get; set; }
    public string? Image { get; set; } = null;
    public string Date { get; set; }

    public ReplyDto(string author, string text)
    {
        Author = author;
        Text = text;
        Replies = new List<ReplyDto>();
        Date = DateTime.UtcNow.ToString("o"); // ISO 8601
    }
}