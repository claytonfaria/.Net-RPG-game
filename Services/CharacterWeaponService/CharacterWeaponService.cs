using System.Security.Claims;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Dtos.Weapon;
using dotnet_rpg.Models;
using dotnet_rpg.Services.AuthRepository;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterWeaponService;

public class CharacterWeaponService : ICharacterWeaponService
{
    private readonly DataContext _dataContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IAuthRepository _authRepository;

    public CharacterWeaponService(DataContext dataContext, IHttpContextAccessor httpContextAccessor, IMapper mapper,
        IAuthRepository authRepository)
    {
        _dataContext = dataContext;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _authRepository = authRepository;
    }

    public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(int characterId, AddWeaponDto newWeapon)
    {
        ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();

        try
        {
            var currentUserId = _authRepository.GetUserId();

            Character? character = await _dataContext.Characters.FirstOrDefaultAsync(character =>
                character.Id == characterId && character.User!.Id ==
                currentUserId);

            if (character is null)
            {
                response.Success = false;
                response.Message = "Character not found";
                return response;
            }

            Weapon weapon = new Weapon
            {
                Name = newWeapon.Name,
                Damage = newWeapon.Damage,
                Character = character
            };
            _dataContext.Weapons.Add(weapon);
            await _dataContext.SaveChangesAsync();
            response.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            response.Success = false;
            response.Message = e.Message;
        }

        return response;
    }
}