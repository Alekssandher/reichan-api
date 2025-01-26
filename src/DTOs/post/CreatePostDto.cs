using System.ComponentModel.DataAnnotations;

public class CreatePostDto {
    
    [StringLength(30, MinimumLength = 1, ErrorMessage = "Title chars must be between 1 - 30 chars")]
    public string Title { get; set; }

    [StringLength(600, MinimumLength = 1, ErrorMessage = "Text chars must be between 1 - 600 chars")]
    public string Text { get; set; }
    
    [StringLength(50, ErrorMessage = "Lenght error for image")]
    public string Image { get; set; }

    [StringLength(15, MinimumLength = 1, ErrorMessage = "Category chars must be between 1 - 15 chars")]
    public string Category { get; set;}

   
    public string? Author { get; set; }
    public List<ReplyDto> Replies { get; set; }
    public string Date { get; set; }
    public bool Active { get; set; }
    public int Votes { get; set; }

    public CreatePostDto(string title, string text, string image, string category, string author)
    {
        Title = title;
        Text = text;
        Image = image;
        Category = category;
        Author = author;
        Replies = new List<ReplyDto>();
        Date = DateTime.UtcNow.ToString("o"); // ISO 8601
        Active = true;
        Votes = 0;
    }
}