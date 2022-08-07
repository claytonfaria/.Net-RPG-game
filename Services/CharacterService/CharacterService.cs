using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;
using dotnet_rpg.Services.AuthRepository;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService;

public class CharacterService : ICharacterService
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;
    private readonly IAuthRepository _authRepository;

    public CharacterService(IMapper mapper, DataContext context, IAuthRepository authRepository)
    {
        _mapper = mapper;
        _context = context;
        _authRepository = authRepository;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
    {
        var dbCharacters = await _context
            .Characters
            .Where(c => c.User.Id == _authRepository.GetUserId())
            .Include(c => c.Skills)
            .Include(c => c.Weapon)
            .ToListAsync();
        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
        {
            Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList()
        };

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
    {
        var dbCharacter = await _context.Characters
            .Include(c => c.Skills)
            .Include(c => c.Weapon)
            .FirstOrDefaultAsync(character =>
                character.Id == id && character.User.Id == _authRepository.GetUserId());
        var serviceResponse = new ServiceResponse<GetCharacterDto>
        {
            Data = _mapper.Map<GetCharacterDto>(dbCharacter)
        };
        return serviceResponse;
    }

    public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
    {
        var character = _mapper.Map<Character>(newCharacter);
        character.User = await _context.Users.FirstAsync(user => user.Id == _authRepository.GetUserId());

        _context.Characters.Add(character);

        await _context.SaveChangesAsync();

        var serviceResponse = new ServiceResponse<List<GetCharacterDto>>
        {
            Data = await _context.Characters
                .Where(c => c.User.Id == _authRepository.GetUserId())
                .Include(c => c.Skills)
                .Include(c => c.Weapon)
                .Select(c => _mapper.Map<GetCharacterDto>(c))
                .ToListAsync()
        };

        return serviceResponse;
    }

    public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(int id, UpdateCharacterDto updatedCharacter)
    {
        var serviceResponse = new ServiceResponse<GetCharacterDto>();

        try
        {
            var character =
                await _context.Characters
                    // this includes the User to the `character` variable - Not using in the example below, but it allows to access character.User
                    .Include(c => c.User)
                    .FirstAsync(
                        character => character.Id == id && character.User.Id == _authRepository.GetUserId());

            _mapper.Map(updatedCharacter, character);

            await _context.SaveChangesAsync();

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
            var character =
                await _context.Characters.FirstAsync(
                    character => character.Id == id && character.User.Id == _authRepository.GetUserId());

            _context.Characters.Remove(character);

            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Characters.Where(user => user.Id == _authRepository.GetUserId())
                .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
        }
        catch (Exception e)
        {
            serviceResponse.Success = false;
            serviceResponse.Message = e.Message;
        }

        return serviceResponse;
    }
}