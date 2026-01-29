using Cinema.Domain.DTOs;
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

    public async Task<PagedResult<BookingListItem>> GetBookingsAsync(
        int page,
        int pageSize,
        string? movieTitle,
        string? login)
    {
        var (bookings, total) = await _repo.GetPagedAsync(page, pageSize, movieTitle, login);

        return new PagedResult<BookingListItem>
        {
            Items = bookings,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<PagedResult<BookingListItem>> GetBookingsByUserIAsync(
        int page,
        int pageSize,
        Guid userId)
    {
        var (bookings, total) = await _repo.GetByUserIdPagedAsync(page, pageSize, userId);

        return new PagedResult<BookingListItem>
        {
            Items = bookings,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }
}
