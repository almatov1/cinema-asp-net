using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DTOs;

public class LoginRequest
{
    [Required]
    [MinLength(3)]
    public string Login { get; set; } = null!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;
}
