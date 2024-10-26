using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApiRPG.Models;

public class Skill
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100), MinLength(3)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(3, 5, ErrorMessage = "Skills's damage must lie between 3 and 5")]
    [DefaultValue(3)]
    public double Damage { get; set; }


    public List<Character> Characters { get; set; } = new();
}
