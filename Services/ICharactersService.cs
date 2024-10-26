using Microsoft.AspNetCore.Mvc;
using WebApiRPG.DTOs;

namespace WebApiRPG.Services;

public interface ICharactersService
{
    public Task<ServiceResponse<List<GetCharacterDTO>>> GetCharacters(string userId);
    public Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int id, string userId);
    public Task<ServiceResponse<GetCharacterDTO>> AddNewCharacter(PostCharacterDTO characterDTO, string userId);
    public Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(int id, PostCharacterDTO updatedCharacterDTO, string userId);
    public Task<ServiceResponse<string>> DeleteCharacter(int id, string userId);

    public Task<ServiceResponse<GetCharacterDTO>> CreateNewWeapon(int characterId, AddWeaponDTO weapon, string userId);
    public Task<ServiceResponse<GetCharacterDTO>> AssignExistingWeapon(int characterId, int weaponId, string userId);

    public Task<ServiceResponse<GetSkillDTO>> CreateNewSkill(AddSkillDTO skillDTO);
    public Task<ServiceResponse<GetCharacterDTO>> AssignSkillToCharacter(int characterId, int skillId, string userId);
    public Task<ServiceResponse<List<GetSkillDTO>>> ListSkillsOfCharacter(int characterId, string userId);

    public Task<ServiceResponse<string>> StartMatch(int attackerId, int targetId, string userId);
    public Task<ServiceResponse<AttackResultDTO>> WeaponAttack(WeaponAttackDTO attackDTO, string userId);
    public Task<ServiceResponse<AttackResultDTO>> SkillAttack(SkillAttackDTO attackDTO, string userId);


    public Task<ServiceResponse<List<CharacterLeaderBoardDTO>>> GetCharactersLeaderBoard();

    public Task ResetInGameStatus();

}