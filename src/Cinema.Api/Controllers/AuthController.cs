using Microsoft.AspNetCore.Mvc;
using Cinema.Application.Services;
using Cinema.Domain.DTOs;
using Cinema.Api.Services;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(AuthService authService, JwtTokenService jwtService) : ControllerBase
{
    private readonly AuthService _authService = authService;
    private readonly JwtTokenService _jwtService = jwtService;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _authService.ValidateUserAsync(request.Login, request.Password);
        var token = _jwtService.GenerateToken(user);
        return Ok(new { access_token = token });
    }
}
