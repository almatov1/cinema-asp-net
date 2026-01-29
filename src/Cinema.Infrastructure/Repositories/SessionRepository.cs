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
}
