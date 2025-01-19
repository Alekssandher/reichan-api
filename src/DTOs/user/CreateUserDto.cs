using System.ComponentModel.DataAnnotations;

public class CreateUserDto
{
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Name chars must be between 3 - 30")]
    public string Name { get; set; }

    [StringLength(maximumLength: 35, ErrorMessage = "Max length is 35")]
    public string Email { get; set; }

    [StringLength(50, MinimumLength = 5, ErrorMessage = "Password must be beteween 3 - 50")]
    public string Password { get; set; } 
}