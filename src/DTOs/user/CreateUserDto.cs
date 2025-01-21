using System.ComponentModel.DataAnnotations;

public class CreateUserDto
{
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Name chars must be between 3 - 30 chars")]
    public string Nick { get; set; }

    [StringLength(maximumLength: 60, ErrorMessage = "Max length is 60 chars")]
    public string PublicKey { get; set; }

    [StringLength(150, ErrorMessage = "Max length is 50 chars")]
    public string Image { get; set; } 

}