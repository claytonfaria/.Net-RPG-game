using System.Security.Claims;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Dtos.Weapon;
using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.WeaponService;

public class WeaponService : IWeaponService
{
    private readonly DataContext _dataContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public WeaponService(DataContext dataContext, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _dataContext = dataContext;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
    {
        ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();

        try
        {
            var currentUserId =
                int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));

            Character? character = await _dataContext.Characters.FirstOrDefaultAsync(character =>
                character.Id == newWeapon.CharacterId && character.User!.Id ==
                currentUserId);

            if (character == null)
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