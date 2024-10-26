using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiRPG.Data;
using WebApiRPG.DTOs;
using WebApiRPG.Models;

namespace WebApiRPG.Services;

public class CharactersService : ICharactersService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CharactersService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    #region Character
    public async Task<ServiceResponse<List<GetCharacterDTO>>> GetCharacters(string userId)
    {
        var characterDTOs = _mapper.Map<List<GetCharacterDTO>>(
                            await GetUsersCharactersQuery(userId)
                            .ToListAsync());

        return new ServiceResponse<List<GetCharacterDTO>>
        {
            Data = characterDTOs
        };
    }

    public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterById(int characterId, string userId)
    {
        var character = await GetUsersCharactersQuery(userId)
                        .FirstOrDefaultAsync(c => c.Id == characterId);

        if (character == null)
        {
            return new ServiceResponse<GetCharacterDTO>
            {
                Success = false,
                Message = "Character not found."
            };
        }
        return new ServiceResponse<GetCharacterDTO>
        {
            Data = _mapper.Map<GetCharacterDTO>(character)
        };
    }

    public async Task<ServiceResponse<GetCharacterDTO>> AddNewCharacter(
        PostCharacterDTO characterDTO, string userId)
    {
        if (await _context.Characters
            .AnyAsync(c => c.Name == characterDTO.Name))
        {
            return new ServiceResponse<GetCharacterDTO>
            {
                Success = false,
                Message = "Character with this Name already exists."
            };
        }

        var character = _mapper.Map<Character>(characterDTO);
        character.UserId = new Guid(userId);

        await _context.AddAsync(character);
        await _context.SaveChangesAsync();

        return new ServiceResponse<GetCharacterDTO>
        {
            Data = _mapper.Map<GetCharacterDTO>(character)
        };
    }

    public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(
        int id, PostCharacterDTO updatedCharacterDTO, string userId)
    {
        var character = await GetUsersCharactersQuery(userId)     
                        .FirstOrDefaultAsync(c => c.Id == id);

        if (character == null)
        {
            return new ServiceResponse<GetCharacterDTO>
            {
                Success = false,
                Message = "Character not found."
            };
        }

        _mapper.Map(updatedCharacterDTO, character);
        await _context.SaveChangesAsync();

        return new ServiceResponse<GetCharacterDTO>
        {
            Data = _mapper.Map<GetCharacterDTO>(character)
        };
    }

    public async Task<ServiceResponse<string>>
        DeleteCharacter(int characterId, string userId)
    {
        var character = await _context.Characters
                        .FirstOrDefaultAsync(c => c.Id == characterId &&
                        c.UserId.ToString() == userId);

        if (character == null)
        {
            return new ServiceResponse<string>
            {
                Success = false,
                Message = "Character not found."
            };
        }

        _context.Remove(character);
        await _context.SaveChangesAsync();

        return new ServiceResponse<string>
        {
            Data = "Character removed successfully"
        };
    }
    #endregion

    #region Weapons

   public async Task<ServiceResponse<GetCharacterDTO>> CreateNewWeapon(
        int characterId, AddWeaponDTO weaponDTO, string userId)
   {
        var character = await GetUsersCharactersQuery(userId)
                        .FirstOrDefaultAsync(c => c.Id == characterId);

        if (character == null)
        {
            return new ServiceResponse<GetCharacterDTO>
            {
                Success = false,
                Message = "Character not found."
            };
        }

        var currentWeapon = character.Weapon;
        if (currentWeapon != null)
        {
            currentWeapon.CharacterId = null;
            currentWeapon.Character = null;
        }
        await _context.SaveChangesAsync();

        Weapon weapon = _mapper.Map<Weapon>(weaponDTO);
        weapon.Character = character;
        await _context.AddAsync(weapon);

        character.Weapon = weapon;
        await _context.SaveChangesAsync();

        return new ServiceResponse<GetCharacterDTO>()
        {
            Data = _mapper.Map<GetCharacterDTO>(character)
        };
    }

    public async Task<ServiceResponse<GetCharacterDTO>> AssignExistingWeapon(
        int characterId, int weaponId, string userId)
    {
        var character = await GetUsersCharactersQuery(userId)
                        .FirstOrDefaultAsync(c => c.Id == characterId);

        if (character == null)
        {
            return new ServiceResponse<GetCharacterDTO>
            {
                Success = false,
                Message = "Character not found."
            };
        }

        //weapon should have no owner
        var weapon = await _context.Weapons
                    .FirstOrDefaultAsync(w => w.Id == weaponId && 
                    w.CharacterId == null);

        if (weapon == null)
        {
            return new ServiceResponse<GetCharacterDTO>
            {
                Success = false,
                Message = "Weapon not found or already belongs to someone."
            };
        }

        var currentWeapon = character.Weapon;
        if(currentWeapon != null)
        {
            currentWeapon.CharacterId = null;
            currentWeapon.Character = null;
        }

        weapon.Character = character;
        character.Weapon = weapon;

        await _context.SaveChangesAsync();

        return new ServiceResponse<GetCharacterDTO>()
        {
            Data = _mapper.Map<GetCharacterDTO>(character)
        };
    }

    #endregion

    #region Skills
    public async Task<ServiceResponse<GetSkillDTO>> CreateNewSkill(AddSkillDTO skillDTO)
    {
        Skill skill = _mapper.Map<Skill>(skillDTO);
        
        await _context.AddAsync(skill);
        await _context.SaveChangesAsync();

        return new ServiceResponse<GetSkillDTO>()
        {
            Data = _mapper.Map<GetSkillDTO>(skill)
        };

    }

    public async Task<ServiceResponse<GetCharacterDTO>> AssignSkillToCharacter(
        int characterId, int skillId, string userId)
    {
        var character = await GetUsersCharactersQuery(userId)
                        .FirstOrDefaultAsync(c => c.Id == characterId);

        if (character == null)
        {
            return new ServiceResponse<GetCharacterDTO>
            {
                Success = false,
                Message = "Character not found."
            };
        }

        var skill = await _context.Skills
                    .FindAsync(skillId);

        if (skill == null)
        {
            return new ServiceResponse<GetCharacterDTO>
            {
                Success = false,
                Message = "Skill not found."
            };
        }

        skill.Characters.Add(character);
        character.Skills.Add(skill);

        await _context.SaveChangesAsync();

        return new ServiceResponse<GetCharacterDTO>()
        {
            Data = _mapper.Map<GetCharacterDTO>(character)
        };
    }

    public async Task<ServiceResponse<List<GetSkillDTO>>> ListSkillsOfCharacter(int characterId, string userId)
    {
        var character = await GetUsersCharactersQuery(userId)
                       .FirstOrDefaultAsync(c => c.Id == characterId);

        if (character == null)
        {
            return new ServiceResponse<List<GetSkillDTO>>
            {
                Success = false,
                Message = "Character not found."
            };
        }

        return new ServiceResponse<List<GetSkillDTO>> ()
        {
            Data = _mapper.Map<List<GetSkillDTO>>(character.Skills)
        };
    }

    #endregion

    #region Matches
    public async Task<ServiceResponse<string>> StartMatch(int attackerId, int targetId, string userId)
    {
        var attacker = await _context.Characters
            .FirstOrDefaultAsync(c => c.Id == attackerId && c.UserId.ToString() == userId); ;
        var target = await _context.Characters.FindAsync(targetId);

        if (attacker == null || target == null)
        {
            return new ServiceResponse<string>
            {
                Success = false,
                Message = "One or both characters do not exist."
            };
        }

        if(attacker.InGame || target.InGame)
        {
            return new ServiceResponse<string>
            {
                Success = false,
                Message = "One or both characters are already involved in a match."
            };
        }

        attacker.CurrentHealth = attacker.Health;
        target.CurrentHealth = target.Health;

        attacker.InGame = true;
        target.InGame = true;

        await _context.SaveChangesAsync();
        return new ServiceResponse<string>
        {
            Data = "The match has started"
        };
    }

    public async Task<ServiceResponse<AttackResultDTO>> WeaponAttack(WeaponAttackDTO attackDTO, string userId)
    {
        var attacker = await _context.Characters
            .Include(c => c.Weapon)
            .FirstOrDefaultAsync(c => c.Id == attackDTO.AttackerId && c.UserId.ToString() == userId);
        var target = await _context.Characters.FindAsync(attackDTO.TargetId);

        if (attacker == null || target == null)
        {
            return new ServiceResponse<AttackResultDTO>
            {
                Success = false,
                Message = "One or both characters do not exist."
            };
        }

        if (!attacker.InGame || !target.InGame)
        {
            return new ServiceResponse<AttackResultDTO>
            {
                Success = false,
                Message = "One or both characters are not in game."
            };
        }

        double damage = attacker.Strength * 0.7 + attacker.Weapon.Damage * 0.5;
        target.CurrentHealth -= damage;

        if(target.CurrentHealth <= 0)
        {
            target.CurrentHealth = 0;
            attacker.TotalVictories += 1;
            attacker.InGame = false;
            target.InGame = false;
        }

        await _context.SaveChangesAsync();
        return new ServiceResponse<AttackResultDTO>
        {
            Data = new AttackResultDTO
            {
                AttackerId = attackDTO.AttackerId,
                TargetId = attackDTO.TargetId,
                Damage = damage,
                TargetHealth = target.CurrentHealth
            }
        };
    }

    public async Task<ServiceResponse<AttackResultDTO>> SkillAttack(SkillAttackDTO attackDTO, string userId)
    {
        var attacker = await _context.Characters
            .Include(c => c.Skills)
            .FirstOrDefaultAsync(c => c.Id == attackDTO.AttackerId && c.UserId.ToString() == userId); ;
        var target = await _context.Characters.FindAsync(attackDTO.TargetId);

        if (attacker == null || target == null)
        {
            return new ServiceResponse<AttackResultDTO>
            {
                Success = false,
                Message = "One or both characters do not exist."
            };
        }

        if (!attacker.InGame || !target.InGame)
        {
            return new ServiceResponse<AttackResultDTO>
            {
                Success = false,
                Message = "One or both characters are not in game."
            };
        }

        var skill = attacker.Skills.FirstOrDefault(s => s.Id == attackDTO.SkillId);
        if(skill == null)
        {
            return new ServiceResponse<AttackResultDTO>
            {
                Success = false,
                Message = "Character doesn't have required skill."
            };
        }

        double damage = attacker.Strength * 0.7 + skill.Damage;
        target.CurrentHealth -= damage;

        if (target.CurrentHealth <= 0)
        {
            target.CurrentHealth = 0;
            attacker.TotalVictories += 1;
            attacker.InGame = false;
            target.InGame = false;
        }

        await _context.SaveChangesAsync();
        return new ServiceResponse<AttackResultDTO>
        {
            Data = new AttackResultDTO
            {
                AttackerId = attackDTO.AttackerId,
                TargetId = attackDTO.TargetId,
                Damage = damage,
                TargetHealth = target.CurrentHealth
            }
        };
    }

    #endregion

    public async Task<ServiceResponse<List<CharacterLeaderBoardDTO>>> GetCharactersLeaderBoard()
    {
        var characters = await _context.Characters
            .Include(c => c.User)
            .Select(c => new CharacterLeaderBoardDTO
            {
                Name = c.Name,
                Health = c.Health,
                Strength = c.Strength,
                TotalVictories = c.TotalVictories,
                Username = c.User.Username
            })
            .OrderByDescending(c => c.TotalVictories)
            .ToListAsync();

        return new ServiceResponse<List<CharacterLeaderBoardDTO>>
        {
            Data = characters
        };

    }

    public async Task ResetInGameStatus()
    {
        var characters = await _context.Characters
            .Where(c => c.InGame == true)
            .ToListAsync();

        foreach(var character in characters)
        {
            character.InGame = false;
        }

        await _context.SaveChangesAsync();
    }

    private IQueryable<Character> GetUsersCharactersQuery(string userId)
    {
        return _context.Characters
            .Where(c => c.UserId.ToString() == userId)
            .Include(c => c.Weapon)
            .Include(c => c.Skills);
    }

    
}
