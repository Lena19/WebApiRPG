using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiRPG.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3, 
    ErrorMessage = "Name length must have between 3 and 100 characters.")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;


    public List<Character> Characters { get; set; }

    public User()
    {
        Id = Guid.NewGuid();
    }

}
