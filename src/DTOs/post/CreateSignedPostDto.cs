using System.ComponentModel.DataAnnotations;
using System.Text;

public class CreateSignedPostDto {
    [Required(ErrorMessage = "Public Key is missing")]
    public string PublicKey { get; set; }

    [StringLength(30, MinimumLength = 1, ErrorMessage = "Title chars must be between 1 - 30 chars")]
    public string Title { get; set; }

    [StringLength(600, MinimumLength = 1, ErrorMessage = "Text chars must be between 1 - 600 chars")]
    public string Text { get; set; }
    
    [StringLength(150, ErrorMessage = "Lenght error for image")]
    public string Image { get; set; }

    [StringLength(15, MinimumLength = 1, ErrorMessage = "Category chars must be between 1 - 15 chars")]
    public string Category { get; set;}

   
    public string? Author { get; set; }

    [StringLength(500, MinimumLength = 1, ErrorMessage = "Signature lenght limit reached")]
    public string Signature { get; set; }

    public List<ReplyDto> Replies { get; set; }
    public DateTime CreatedAt { get; set; }

    public bool Active { get; set; }
    public int Votes { get; set; }

    public CreateSignedPostDto(string publicKey,string title, string text, string image, string category, string author, string signature)
    {
        PublicKey = publicKey;
        Title = title;
        Text = text;
        Image = image;
        Category = category.ToLower();
        Author = author;
        Signature = signature;
        Replies = new List<ReplyDto>();
        CreatedAt = DateTime.UtcNow; // ISO 8601
        Active = true;
        Votes = 0;
    }

    public string GetFormatedContent (){
        string data = Title + Text + Image + Category + CreatedAt;
        return data;
    }

    public string ConvertBase64ToPem(string base64Key)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("-----BEGIN PUBLIC KEY-----");
        int lineLength = 64;
        for (int i = 0; i < base64Key.Length; i += lineLength)
        {
            sb.AppendLine(base64Key.Substring(i, Math.Min(lineLength, base64Key.Length - i)));
        }
        sb.AppendLine("-----END PUBLIC KEY-----");
        return sb.ToString();
    }
}