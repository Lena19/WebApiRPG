using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApiRPG.Models;

public class Weapon
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50), MinLength(3)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.3, 2, ErrorMessage = "Weapon's damage must lie between 0.3 and 2")]
    [DefaultValue(0.3)]
    public double Damage { get; set; }


    public int? CharacterId { get; set; }

    public Character? Character { get; set; }
}
