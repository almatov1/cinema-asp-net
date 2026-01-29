using Cinema.Api.Attributes;
using Cinema.Application.Services;
using Cinema.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/admin/users")]
[AuthorizeRoles(Role.Admin)]
public class AdminUsersController(AdminUserService service) : ControllerBase
{
    private readonly AdminUserService _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 30)
    {
        if (page < 1 || pageSize is < 1 or > 100)
            return BadRequest("Invalid paging");

        var result = await _service.GetUsersAsync(page, pageSize);

        return Ok(result);
    }
}
