using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;
using dotnet_rpg.Services.CharacterService;
using dotnet_rpg.Services.CharacterSkillService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers;

[Authorize]
[Route("api/characters/{characterId:int}/skills")]
[ApiController]
public class CharacterSkillController : ControllerBase
{
    private readonly ICharacterService _characterService;
    private readonly ILogger<CharacterSkillController> _logger;
    private readonly ICharacterSkillService _characterSkillService;

    public CharacterSkillController(ICharacterService characterService, ILogger<CharacterSkillController> logger,
        ICharacterSkillService characterSkillService)
    {
        _characterService = characterService;
        _logger = logger;
        _characterSkillService = characterSkillService;
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddCharacterSkill(int characterId,
        AddCharacterSkillDto newCharacterSkill)
    {
        return Ok(await _characterSkillService.AddCharacterSkill(characterId, newCharacterSkill));
    }
}