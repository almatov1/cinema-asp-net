using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DTOs;

public class CreateSessionRequest
{
    [Required]
    public string MovieTitle { get; set; } = string.Empty;
    [Required]

    public DateTime DateTime { get; set; }
}
