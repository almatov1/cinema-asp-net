using Microsoft.AspNetCore.Mvc;
using Cinema.Application.Services;
using Cinema.Domain.DTOs;
using Cinema.Api.Attributes;
using Cinema.Domain.Enums;

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
}
