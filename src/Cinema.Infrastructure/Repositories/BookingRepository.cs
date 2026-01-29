using Cinema.Domain.Entities;
using Cinema.Domain.Interfaces;
using Cinema.Infrastructure.Data;
using Dapper;
using System.Data;

namespace Cinema.Infrastructure.Repositories;

public sealed class BookingRepository(DbConnectionFactory factory) : IBookingRepository
{
    private readonly DbConnectionFactory _factory = factory;

    public async Task<Guid> CreateAsync(Booking booking)
    {
        using IDbConnection db = _factory.CreateConnection();

        const string sql = """
            INSERT INTO bookings (session_id, seat_number)
            VALUES (@SessionId, @SeatNumber)
            RETURNING id;
        """;

        try { return await db.ExecuteScalarAsync<Guid>(sql, booking); }
        catch { throw new InvalidOperationException("This seat is already booked for this session"); }
    }
}
