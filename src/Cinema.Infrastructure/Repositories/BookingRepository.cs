using Cinema.Domain.DTOs;
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
    public async Task<(IEnumerable<BookingListItem> Bookings, int Total)> GetPagedAsync(
        int page,
        int pageSize,
        string? movieTitle = null,
        string? login = null)
    {
        using IDbConnection db = _factory.CreateConnection();

        var offset = (page - 1) * pageSize;

        const string sql = """
            SELECT 
                b.id,
                b.seat_number,
                b.created_at,
                b.session_id,
                s.movie_title,
                s.date_time,
                b.user_id,
                u.login
            FROM bookings b
            JOIN users u ON u.id = b.user_id
            JOIN sessions s ON s.id = b.session_id
            WHERE (@MovieTitle IS NULL OR s.movie_title ILIKE '%' || @MovieTitle || '%')
            AND (@Login IS NULL OR u.login ILIKE '%' || @Login || '%')
            ORDER BY b.created_at DESC;

            SELECT COUNT(*) 
            FROM bookings b
            JOIN users u ON u.id = b.user_id
            JOIN sessions s ON s.id = b.session_id
            WHERE (@MovieTitle IS NULL OR s.movie_title ILIKE '%' || @MovieTitle || '%')
            AND (@Login IS NULL OR u.login ILIKE '%' || @Login || '%');
        """;

        using var multi = await db.QueryMultipleAsync(sql, new
        {
            pageSize,
            offset,
            MovieTitle = movieTitle,
            Login = login
        });

        var bookings = await multi.ReadAsync<BookingListItem>();
        var total = await multi.ReadSingleAsync<int>();

        return (bookings, total);
    }
}
