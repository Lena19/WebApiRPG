using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApiRPG.DTOs;

public class PostCharacterDTO
{
    [Required]
    [MinLength(3), MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(10, 15, ErrorMessage = "Health must be between 10 and 15.")]
    [DefaultValue(10)]
    public double Health { get; set; }

    [Required]
    [Range(3, 6, ErrorMessage = "Strength must be between 3 and 6.")]
    [DefaultValue(3)]
    public double Strength { get; set; }
}
