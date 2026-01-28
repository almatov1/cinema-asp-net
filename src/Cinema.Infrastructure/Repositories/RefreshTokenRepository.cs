using System.Data;
using Cinema.Domain.Entities;
using Cinema.Domain.Interfaces;
using Cinema.Infrastructure.Data;
using Dapper;

namespace Cinema.Infrastructure.Repositories;

public class RefreshTokenRepository(DbConnectionFactory factory) : IRefreshTokenRepository
{
    private readonly DbConnectionFactory _factory = factory;

    public async Task CreateAsync(RefreshToken token)
    {
        using IDbConnection db = _factory.CreateConnection();

        const string sql = """
            INSERT INTO refresh_tokens
            (id, user_id, token, expires_at)
            VALUES (@Id, @UserId, @Token, @ExpiresAt);
        """;

        await db.ExecuteAsync(sql, token);
    }

    public async Task<RefreshToken?> GetAsync(string token)
    {
        using IDbConnection db = _factory.CreateConnection();

        const string sql = """
            SELECT *
            FROM refresh_tokens
            WHERE token = @token
              AND revoked_at IS NULL;
        """;

        return await db.QueryFirstOrDefaultAsync<RefreshToken>(
            sql, new { token });
    }

    public async Task RevokeAsync(Guid id)
    {
        using IDbConnection db = _factory.CreateConnection();

        await db.ExecuteAsync("""
            UPDATE refresh_tokens
            SET revoked_at = now()
            WHERE id = @id;
        """, new { id });
    }
}
