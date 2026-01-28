using Npgsql;
using System.Data;

namespace Cinema.Infrastructure.Data;

public class DbConnectionFactory(string connectionString)
{
    private readonly string _connectionString = connectionString;

    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}
