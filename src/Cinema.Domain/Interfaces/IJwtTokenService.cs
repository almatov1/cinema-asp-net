using Cinema.Domain.Entities;

namespace Cinema.Domain.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
}