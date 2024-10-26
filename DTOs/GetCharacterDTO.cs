using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebApiRPG.Models;

namespace WebApiRPG.DTOs;

public class GetCharacterDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Health { get; set; }
    public double CurrentHealth { get; set; }
    public double Strength { get; set; }
    public bool InGame { get; set; }

    public GetWeaponDTO? Weapon { get; set; }
    public List<GetSkillDTO>? Skills { get; set; }
}
