using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiRPG.DTOs;
using WebApiRPG.Models;
using WebApiRPG.Services;

namespace WebApiRPG.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CharactersController : ControllerBase
{
    private readonly ICharactersService _charactersService;

    public CharactersController(ICharactersService charactersService)
    {
        _charactersService = charactersService;
    }

    #region Character

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> GetCharacters()
    {
        var response = await _charactersService
                       .GetCharacters(GetUserId());

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> GetCharacterById(int id)
    {
        var response = await _charactersService
            .GetCharacterById(id, GetUserId());

        if (!response.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> 
        AddNewCharacter([FromBody]PostCharacterDTO characterDTO)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _charactersService
            .AddNewCharacter(characterDTO, GetUserId());

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> 
        UpdateCharacter(int id, [FromBody]PostCharacterDTO updatedCharacterDTO)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var response = await _charactersService
            .UpdateCharacter(id, updatedCharacterDTO, GetUserId());

        if (!response.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<string>>> DeleteCharacter(int id)
    {
        var response = await _charactersService
            .DeleteCharacter(id, GetUserId());

        if (!response.Success)
        {
            return NotFound(response);
        }

        return Ok(response);
    }
    #endregion

    #region Weapons
    [HttpPost("{characterId}/weapons")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> CreateNewWeapon(
        int characterId, AddWeaponDTO weapon)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _charactersService
            .CreateNewWeapon(characterId, weapon, GetUserId());

        if(!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("{characterId}/weapons/assign-existing")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> AssignExistingWeapon(
        int characterId, [FromQuery]int weaponId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _charactersService
            .AssignExistingWeapon(characterId, weaponId, GetUserId());

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }


    #endregion

    #region Skills

    [HttpPost("skills/new")]
    public async Task<ActionResult<ServiceResponse<GetSkillDTO>>> CreateNewSkill(AddSkillDTO skillDTO)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _charactersService.CreateNewSkill(skillDTO);
        
        if(!response.Success)
        {
            return BadRequest(response.Message);
        }

        return Ok(response);
    }

    [HttpPost("{characterId}/skills/assign")]
    public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> AssignSkillToCharacter(int characterId, [FromQuery]int skillId)
    {
        var response = await _charactersService.AssignSkillToCharacter(characterId, skillId, GetUserId());

        if (!response.Success)
        {
            return BadRequest(response.Message);
        }

        return Ok(response);
    }

    [HttpGet("{characterId}/skills")]
    public async Task<ActionResult<ServiceResponse<List<GetSkillDTO>>>> ListSkillsOfCharacter(int characterId)
    {

        var response = await _charactersService.ListSkillsOfCharacter(characterId, GetUserId());

        if (!response.Success)
        {
            return BadRequest(response.Message);
        }

        return Ok(response);
    }


    #endregion

    #region Matches

    [HttpGet("start-match")]
    public async Task<ActionResult<ServiceResponse<string>>> StartMatch(int attackerId, int targetId)
    {
        var response = await _charactersService.StartMatch(attackerId, targetId, GetUserId());
        
        if(!response.Success)
        {
            return BadRequest(response.Message);
        }

        return Ok(response);
    }

    [HttpPost("weapon-attack")]
    public async Task<ActionResult<ServiceResponse<AttackResultDTO>>> WeaponAttack(WeaponAttackDTO attackDTO)
    {
        var response = await _charactersService.WeaponAttack(attackDTO, GetUserId());

        if (!response.Success)
        {
            return BadRequest(response.Message);
        }

        return Ok(response);
    }

    [HttpPost("skill-attack")]
    public async Task<ActionResult<ServiceResponse<AttackResultDTO>>> SkillAttack(SkillAttackDTO attackDTO)
    {
        var response = await _charactersService.SkillAttack(attackDTO, GetUserId());

        if (!response.Success)
        {
            return BadRequest(response.Message);
        }

        return Ok(response);
    }

    #endregion

    [HttpGet("leaderboard")]
    [AllowAnonymous]
    public async Task<ActionResult<ServiceResponse<List<CharacterLeaderBoardDTO>>>>
        GetCharactersLeaderboard()
    {
        var response = await _charactersService.GetCharactersLeaderBoard();
        
        if(!response.Success)
        {
            return BadRequest(response.Message);
        }

        return Ok(response);
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
