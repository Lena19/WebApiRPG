using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApiRPG.DTOs;

public class GetWeaponDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Damage { get; set; }
}