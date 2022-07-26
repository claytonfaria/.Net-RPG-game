using System.ComponentModel.DataAnnotations;
using dotnet_rpg.Models;

namespace dotnet_rpg.Dtos.Character;

public class AddCharacterDto
{
    [Required, MinLength(3)]
    public string Name { get; set; }
    
    public int HitPoints { get; set; } = 10;
 
    public int Strength { get; set; } = 10;
 
    public int Intelligence { get; set; } = 10;
    
    public int Defense { get; set; } = 10;

    public RpgClass Class { get; set; } = RpgClass.Knight;
}