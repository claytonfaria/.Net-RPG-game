using System.ComponentModel.DataAnnotations;

namespace dotnet_rpg.Dtos.User;

public class UserRegisterDto
{
    [Required]
    public string? Username { get; set; }
    
    [Required]
    public string? Password { get; set; }
}