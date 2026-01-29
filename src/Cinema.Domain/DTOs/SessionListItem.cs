namespace Cinema.Domain.DTOs;

public class SessionListItem
{
    public Guid Id { get; set; }
    public string MovieTitle { get; set; } = default!;
    public DateTime DateTime { get; set; }
    public DateTime CreatedAt { get; set; }
}
