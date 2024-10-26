using System.ComponentModel.DataAnnotations;

namespace WebApiRPG.DTOs;

public class UserLoginByUsernameDTO
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
