using DbUp;
using System.Reflection;

namespace Cinema.Infrastructure.Data;

public static class MigrationRunner
{
    public static void Run(string connectionString)
    {
        var upgrader =
            DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(
                    Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            throw result.Error;
        }
    }
}
