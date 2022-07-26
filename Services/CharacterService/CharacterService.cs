using AutoMapper;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;

namespace dotnet_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly IMapper _mapper;

    private static readonly List<Character> Characters = new List<Character>()
    {
        new Character(), new Character { Name = "Samu", Id = 1, }
    };

    public CharacterService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
        {
            Data = Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList()
        };

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var character = Characters.FirstOrDefault(character => character.Id == id);
        var serviceResponse = new ServiceResponse<GetCharacterDto>
        {
            Data = _mapper.Map<GetCharacterDto>(character)
        };
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var character = _mapper.Map<Character>(newCharacter);

        character.Id = Characters.Max(c => c.Id) + 1;
        Characters.Add(character);

        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
        {
            Data = Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList()
        };

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(int id, UpdateCharacterDto updatedCharacter)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();

        try
        {
            var character = Characters.First(character => character.Id == id);

            _mapper.Map(updatedCharacter, character);

            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
    {
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

        try
        {
            var character = Characters.First(character => character.Id == id);
            
            Characters.Remove(character);
            
            serviceResponse.Data = Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }
}