namespace Cinema.Domain.Entities;

public class Session
{
    public Guid Id { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Booking> Bookings { get; set; } = Array.Empty<Booking>();
}
