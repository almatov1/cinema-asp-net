using Cinema.Domain.Entities;
using Cinema.Domain.Interfaces;

namespace Cinema.Application.Services;

public class BookingService(IBookingRepository repo)
{
    private readonly IBookingRepository _repo = repo;

    public async Task<Booking> CreateBookingAsync(Guid sessionId, int seatNumber, Guid userId)
    {
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            SessionId = sessionId,
            SeatNumber = seatNumber,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.CreateAsync(booking);
        return booking;
    }
}
