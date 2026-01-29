using Microsoft.AspNetCore.Mvc;
using Cinema.Application.Services;
using Cinema.Domain.DTOs;
using Cinema.Api.Attributes;
using Cinema.Domain.Enums;

namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController(BookingService bookingService) : BaseApiController
{
    private readonly BookingService _bookingService = bookingService;

    [HttpPost]
    [AuthorizeRoles(Role.User)]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequest request)
    {
        var booking = await _bookingService.CreateBookingAsync(request.SessionId, request.SeatNumber, UserId);
        return Ok(booking);
    }
}
