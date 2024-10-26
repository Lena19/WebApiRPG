using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApiRPG.DTOs;

public class UserDTO
{
    [Required]
    [StringLength(100, MinimumLength = 3,
    ErrorMessage = "Name length must have between 3 and 100 characters.")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}