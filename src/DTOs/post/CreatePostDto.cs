using System.ComponentModel.DataAnnotations;

public class CreatePostDto {

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(30, MinimumLength = 1, ErrorMessage = "Title chars must be between 1 - 30 chars")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Text is required.")]
    [StringLength(600, MinimumLength = 1, ErrorMessage = "Text chars must be between 1 - 600 chars")]
    public string Text { get; set; }
    
    [Required(ErrorMessage = "Image is required.")]
    [RegularExpression(@"^[\w,\s-]+\.(jpg|jpeg|png|gif|webpm|mp4|ogg)$", ErrorMessage = "Invalid image or video format are allowed.")]
    public string Image { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    [EnumDataType(typeof(PostCategory), ErrorMessage = "Invalid category.")]
    public string Category { get; set;}

   
    public string? Author { get; set; }
    public List<ReplyDto> Replies { get; set; }
    public DateTime CreatedAt { get; set; } // ISO 8601
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
        CreatedAt = DateTime.UtcNow;
        Active = true;
        Votes = 0;
    }
}