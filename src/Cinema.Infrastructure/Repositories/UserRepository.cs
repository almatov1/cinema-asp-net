using Cinema.Domain.DTOs;
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

        try { return await db.ExecuteScalarAsync<Guid>(sql, user); }
        catch { throw new InvalidOperationException("User already exists"); }
    }

    public async Task<User?> GetByLoginAsync(string login)
    {
        using IDbConnection db = _factory.CreateConnection();

        const string sql = """
            SELECT id, login, role, password_hash, created_at
            FROM users
            WHERE login = @login;
        """;

        return await db.QueryFirstOrDefaultAsync<User>(sql, new { login });
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        using IDbConnection db = _factory.CreateConnection();

        const string sql = """
            SELECT id, login, role, created_at
            FROM users
            WHERE id = @id;
        """;

        return await db.QueryFirstOrDefaultAsync<User>(sql, new { id });
    }

    public async Task<(IEnumerable<UserListItem> Users, int Total)> GetPagedAsync(int page, int pageSize)
    {
        using IDbConnection db = _factory.CreateConnection();

        var offset = (page - 1) * pageSize;

        const string sql = """
            SELECT id, login, role, created_at
            FROM users
            ORDER BY created_at DESC
            LIMIT @pageSize OFFSET @offset;

            SELECT COUNT(*) FROM users;
        """;

        using var multi = await db.QueryMultipleAsync(sql, new { pageSize, offset });
        var users = await multi.ReadAsync<UserListItem>();
        var total = await multi.ReadSingleAsync<int>();

        return (users, total);
    }
}
