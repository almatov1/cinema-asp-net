using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DTOs;

public class CreateBookingRequest
{
    [Required]
    public Guid SessionId { get; set; }
    [Required]
    public int SeatNumber { get; set; }
}
