using System.ComponentModel.DataAnnotations;

namespace dotnet_rpg.Dtos.User;

public class UserLoginDto
{
    [Required]
    public string? Username { get; set; }
    
    [Required]
    public string? Password { get; set; }
}