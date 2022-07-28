using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;
using dotnet_rpg.Services.CharacterService;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CharacterController : ControllerBase
{

    private readonly ICharacterService _characterService;
    private readonly ILogger<CharacterController> _logger;

    public CharacterController(ICharacterService characterService, ILogger<CharacterController> logger)
    {
        _characterService = characterService;
        _logger = logger;
    }
    

    // GET: api/characters
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
    {
        _logger.LogInformation("GET: api/characters called at {}", 
            DateTime.UtcNow.ToLongTimeString());
        return Ok(await _characterService.GetAllCharacters());
    }

    // GET: api/characters/5
    [HttpGet("{id:int}", Name = "Get")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> Get(int id)
    {

        var response = await _characterService.GetCharacterById(id);
        if (response.Data != null) return Ok(response);
        response.Success = false;
        return NotFound(response);
    }

    // POST: api/characters
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Post([FromBody] AddCharacterDto newCharacter)
    {

        return Created("created", await _characterService.AddCharacter(newCharacter));
    }

    // PUT: api/characters/5
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResponse<GetCharacterDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ServiceResponse<>))]
    public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> Put(int id, [FromBody] UpdateCharacterDto updatedCharacter)
    {
        var response = await _characterService.UpdateCharacter(id, updatedCharacter);
        if (response.Data != null) return Ok(response);
        response.Success = false;
        return NotFound(response);
    }

    // DELETE: api/characters/5
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Delete(int id)
    {
        var response = await _characterService.DeleteCharacter(id);
        if (response.Data != null) return Ok(response);
        response.Success = false;
        return NotFound(response);
    }
}