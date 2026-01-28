using Microsoft.AspNetCore.Mvc;
using Cinema.Application.Services;
using Cinema.Domain.DTOs;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(UserService userService) : ControllerBase
{
    private readonly UserService _userService = userService;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        try
        {
            var user = await _userService.CreateUserAsync(request.Login, request.Password);
            return Ok(new { user.Id, user.Login, user.CreatedAt });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
