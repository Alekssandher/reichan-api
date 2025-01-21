public class UserDto
{
    public required string Id { get; set; }
    public required string Nick { get; set; }
    public required string PublicKey { get; set; } 
    public required string Image { get; set; } 
    public required Array Posts { get; set; }
}