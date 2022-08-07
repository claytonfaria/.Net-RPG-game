using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Dtos.Weapon;
using dotnet_rpg.Models;
using dotnet_rpg.Services.CharacterWeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers;

[Authorize]
[Route("api/characters/{characterId:int}/weapons")]
[ApiController]
public class CharacterWeaponController : ControllerBase
{
    private readonly ICharacterWeaponService _characterWeaponService;
    private readonly ILogger<CharacterWeaponController> _logger;

    public CharacterWeaponController(ICharacterWeaponService characterWeaponService,
        ILogger<CharacterWeaponController> logger
    )
    {
        _characterWeaponService = characterWeaponService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> Post(int characterId,
        AddWeaponDto newCharacterWeapon)
    {
        return Ok(await _characterWeaponService.AddWeapon(characterId, newCharacterWeapon));
    }
}