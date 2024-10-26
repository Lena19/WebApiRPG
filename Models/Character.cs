using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiRPG.Models;

public class Character
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3,
    ErrorMessage = "Name length must have between 3 and 50 characters.")]

    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(10, 15, ErrorMessage = "Health must be between 10 and 15.")]
    [DefaultValue(10)]
    public double Health { get; set; }

    public double CurrentHealth { get; set; }

    public bool InGame { get; set; } = false;

    public int TotalVictories { get; set; } = 0;



    [Required]
    [Range(3, 6, ErrorMessage = "Strength must be between 3 and 6.")]
    [DefaultValue(3)]
    public double Strength { get; set; }

    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; }


    public int? WeaponId { get; set; }
    public Weapon? Weapon { get; set; }


    public List<Skill> Skills { get; set; } = new();
}
