using Microsoft.AspNetCore.Mvc;
using Cinema.Application.Services;
using Cinema.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(UserService userService) : ControllerBase
{
    private readonly UserService _userService = userService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var user = await _userService.CreateUserAsync(request.Login, request.Password);
        return Ok(new { user.Id, user.Login, user.CreatedAt });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get()
    {
        var userIdClaim = User.FindFirstValue("id");
        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId)) return Unauthorized(new { message = "Invalid user ID in token" });

        var user = await _userService.GetUserByIdAsync(userId);
        return Ok(new { user.Id, user.Login, user.CreatedAt });
    }
}
