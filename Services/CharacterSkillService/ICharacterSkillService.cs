using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;

namespace dotnet_rpg.Services.CharacterSkillService;

public interface ICharacterSkillService
{
    Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(int characterId, AddCharacterSkillDto newCharacterSkill);

}