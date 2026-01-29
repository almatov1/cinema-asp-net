using Cinema.Domain.DTOs;

namespace Cinema.Domain.Entities;

public class Booking
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public Session Session { get; set; } = null!;
    public int SeatNumber { get; set; }
    public Guid UserId { get; set; }
    public UserListItem User { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
