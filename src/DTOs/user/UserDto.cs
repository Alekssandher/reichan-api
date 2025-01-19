using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class UserDto
{
    public required string Id { get; set; } // Identificador único do usuário
    public required string Name { get; set; } // Nome do usuário
    public required string Email { get; set; } // Email do usuário
    public required string Password { get; set; } // Idade do usuário
}