namespace Cinema.Domain.DTOs;

public class UserListItem
{
    public Guid Id { get; set; }
    public string Login { get; set; } = default!;
    public string Role { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
