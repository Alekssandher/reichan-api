using System.ComponentModel.DataAnnotations;

public class CreatePostDto {
    [StringLength(30, MinimumLength = 1, ErrorMessage = "Title chars must be between 1 - 30 chars")]
    public string Title { get; set; }

    [StringLength(600, MinimumLength = 1, ErrorMessage = "Text chars must be between 1 - 600 chars")]
    public string Text { get; set; }
    
    [StringLength(150, ErrorMessage = "Lenght error for image")]
    public string Image { get; set; }

    [StringLength(15, MinimumLength = 1, ErrorMessage = "Category chars must be between 1 - 15 chars")]
    public string Category { get; set;}

    [StringLength(30, MinimumLength = 1, ErrorMessage = "Author chars must be between 1 - 30 chars")]
    public string Author { get; set; }

    [StringLength(500, MinimumLength = 1, ErrorMessage = "Signature lenght limit reached")]
    public string? Signature { get; set; }

    public string Date { get; set; }

    public CreatePostDto(string title, string text, string image, string category, string author)
    {
        Title = title;
        Text = text;
        Image = image;
        Category = category;
        Author = author;
        Date = DateTime.UtcNow.ToString("o"); // ISO 8601
    }
}