using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DotnetLearn.QueryFilterDemo.Tests.Utility;

internal sealed class SqliteInMemoryOptionsProvider : IDisposable
{
    private const string InMemoryConnectionString = "Filename=:memory:";
    private readonly SqliteConnection connection;

    public SqliteInMemoryOptionsProvider()
    {
        connection = new (InMemoryConnectionString);
        connection.Open();

        Options = new DbContextOptionsBuilder()
            .UseSqlite(connection)
            .Options;
    }

    public DbContextOptions Options { get; }

    public void Dispose() => connection.Dispose();
}