using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Dtos.Weapon;
using dotnet_rpg.Models;

namespace dotnet_rpg.Services.CharacterWeaponService;

public interface ICharacterWeaponService
{
    Task<ServiceResponse<GetCharacterDto>> AddWeapon(int characterId, AddWeaponDto newWeapon);
}