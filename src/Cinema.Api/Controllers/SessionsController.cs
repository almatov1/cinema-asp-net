using Microsoft.AspNetCore.Mvc;
using Cinema.Application.Services;
using Cinema.Domain.DTOs;
using Cinema.Api.Attributes;
using Cinema.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionsController(SessionService sessionService) : ControllerBase
{
    private readonly SessionService _sessionService = sessionService;

    [HttpPost]
    [AuthorizeRoles(Role.Manager)]
    public async Task<IActionResult> Create([FromBody] CreateSessionRequest request)
    {
        var session = await _sessionService.CreateSessionAsync(request.MovieTitle, request.DateTime);
        return Ok(session);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll(
     [FromQuery] int page = 1,
     [FromQuery] int pageSize = 20)
    {
        if (page < 1 || pageSize is < 1 or > 100)
            return BadRequest("Invalid paging");

        var result = await _sessionService.GetSessionsAsync(page, pageSize);

        return Ok(result);
    }
}
