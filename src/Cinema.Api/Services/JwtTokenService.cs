using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cinema.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Cinema.Api.Services;

public sealed class JwtTokenService
{
    private readonly string _secret;
    private readonly int _expiresMinutes;

    public JwtTokenService()
    {
        _secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "super-secret-key";
        _expiresMinutes = int.TryParse(Environment.GetEnvironmentVariable("JWT_EXPIRES_MINUTES"), out var m) ? m : 60;
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            ]),
            Expires = DateTime.UtcNow.AddMinutes(_expiresMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
