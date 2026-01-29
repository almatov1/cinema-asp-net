using Microsoft.AspNetCore.Mvc;
using Cinema.Application.Services;
using Cinema.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(UserService userService) : BaseApiController
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
        var user = await _userService.GetUserByIdAsync(UserId);
        return Ok(new { user.Id, user.Login, user.Role, user.CreatedAt });
    }
}
