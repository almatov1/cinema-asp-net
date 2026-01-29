using Cinema.Domain.DTOs;
using Cinema.Domain.Entities;
using Cinema.Domain.Interfaces;
using Cinema.Infrastructure.Data;
using Dapper;
using System.Data;

namespace Cinema.Infrastructure.Repositories;

public sealed class SessionRepository(DbConnectionFactory factory) : ISessionRepository
{
    private readonly DbConnectionFactory _factory = factory;

    public async Task<Guid> CreateAsync(Session session)
    {
        using IDbConnection db = _factory.CreateConnection();

        const string sql = """
            INSERT INTO sessions (movie_title, date_time)
            VALUES (@MovieTitle, @DateTime)
            RETURNING id;
        """;

        return await db.ExecuteScalarAsync<Guid>(sql, session);
    }

    public async Task<(IEnumerable<SessionListItem> Sessions, int Total)> GetPagedAsync(int page, int pageSize)
    {
        using IDbConnection db = _factory.CreateConnection();

        var offset = (page - 1) * pageSize;

        const string sql = """
            SELECT id, movie_title, date_time, created_at
            FROM sessions
            ORDER BY created_at DESC
            LIMIT @pageSize OFFSET @offset;

            SELECT COUNT(*) FROM sessions;
        """;

        using var multi = await db.QueryMultipleAsync(sql,
            new { pageSize, offset });

        var sessions = await multi.ReadAsync<SessionListItem>();
        var total = await multi.ReadSingleAsync<int>();

        return (sessions, total);
    }
}
