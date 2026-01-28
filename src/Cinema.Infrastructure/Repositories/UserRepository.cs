using Cinema.Domain.Entities;
using Cinema.Domain.Interfaces;
using Cinema.Infrastructure.Data;
using Dapper;
using System.Data;

namespace Cinema.Infrastructure.Repositories;

public sealed class UserRepository(DbConnectionFactory factory) : IUserRepository
{
    private readonly DbConnectionFactory _factory = factory;

    public async Task<Guid> CreateAsync(User user)
    {
        using IDbConnection db = _factory.CreateConnection();

        const string sql = """
            INSERT INTO users (login, password_hash)
            VALUES (@Login, @PasswordHash)
            RETURNING id;
        """;

        return await db.ExecuteScalarAsync<Guid>(sql, user);
    }

    public async Task<User?> GetByLoginAsync(string login)
    {
        using IDbConnection db = _factory.CreateConnection();

        const string sql = """
            SELECT id, login, password_hash, created_at
            FROM users
            WHERE login = @login;
        """;

        return await db.QueryFirstOrDefaultAsync<User>(sql, new { login });
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        using IDbConnection db = _factory.CreateConnection();

        const string sql = """
            SELECT id, login, password_hash, created_at
            FROM users
            WHERE id = @id;
        """;

        return await db.QueryFirstOrDefaultAsync<User>(sql, new { id });
    }
}
