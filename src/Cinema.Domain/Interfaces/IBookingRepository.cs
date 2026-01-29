using Cinema.Domain.Entities;

namespace Cinema.Domain.Interfaces;

public interface IBookingRepository
{
    Task<Guid> CreateAsync(Booking booking);
}
