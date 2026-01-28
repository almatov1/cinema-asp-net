namespace Cinema.Domain.DTOs;

public class UserListItemDto
{
    public Guid Id { get; set; }
    public string Login { get; set; } = default!;
    public string Role { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}
