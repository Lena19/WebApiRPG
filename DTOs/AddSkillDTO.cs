using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApiRPG.DTOs;

public class AddSkillDTO
{
    
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
}
