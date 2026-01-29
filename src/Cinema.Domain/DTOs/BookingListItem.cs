namespace Cinema.Domain.DTOs;

public class BookingListItem
{
    public Guid Id { get; set; }
    public int SeatNumber { get; set; }
    public DateTime CreatedAt { get; set; }

    public Guid SessionId { get; set; }
    public string MovieTitle { get; set; } = default!;
    public DateTime DateTime { get; set; }

    public Guid UserId { get; set; }
    public string Login { get; set; } = default!;
}
