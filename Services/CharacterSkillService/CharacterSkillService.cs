using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;
using System.Security.Claims;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Services.AuthRepository;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterSkillService;

public class CharacterSkillService : ICharacterSkillService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IAuthRepository _authRepository;

    public CharacterSkillService(IHttpContextAccessor httpContextAccessor, DataContext context, IMapper mapper, IAuthRepository authRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
        _mapper = mapper;
        _authRepository = authRepository;
    }

    public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(int characterId,
        AddCharacterSkillDto newCharacterSkill)
    {
        var response = new ServiceResponse<GetCharacterDto>();
        try
        {
            var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == characterId && c.User.Id == _authRepository.GetUserId());

            if (character is null)
            {
                response.Success = false;
                response.Message = "Character not found";
                return response;
            }

            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
            if (skill is null)
            {
                response.Success = false;
                response.Message = "Skill not found";
                return response;
            }
            character.Skills.Add(skill);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<GetCharacterDto>(character);
        }
        catch (Exception e)
        {
            response.Success = false;
            response.Message = e.Message;
        }

        return response;
    }
}