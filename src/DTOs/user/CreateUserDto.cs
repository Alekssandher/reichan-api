using System.ComponentModel.DataAnnotations;

public class CreateUserDto
{
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Name chars must be between 3 - 30 chars")]
    public string Nick { get; set; }

    [StringLength(maximumLength: 60, ErrorMessage = "Max length is 60 chars")]
    public string PublicKey { get; set; }

    [StringLength(150, ErrorMessage = "Max length is 50 chars")]
    public string Image { get; set; } 

    public List<UserPostDto> Posts { get; set; }
    public DateTime CreatedAt { get; set; }

    public CreateUserDto(string nick, string publicKey, string image)
    {
        Nick = nick;
        PublicKey = publicKey;
        Image = image;
        
        Posts = new List<UserPostDto>();
        CreatedAt = DateTime.UtcNow; // ISO 8601
    }

}