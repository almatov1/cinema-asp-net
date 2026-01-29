using Cinema.Domain.DTOs;
using Cinema.Domain.Entities;

namespace Cinema.Domain.Interfaces;

public interface IBookingRepository
{
    Task<Guid> CreateAsync(Booking booking);
    Task<(IEnumerable<BookingListItem> Bookings, int Total)> GetPagedAsync(int page, int pageSize, string? movieTitle = null, string? login = null);
    Task<(IEnumerable<BookingListItem> Bookings, int Total)> GetByUserIdPagedAsync(int page, int pageSize, Guid userId);
}
