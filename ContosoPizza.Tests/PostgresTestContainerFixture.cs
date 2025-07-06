using ContosoPizza.Data;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;

using Microsoft.EntityFrameworkCore;

public class PostgresTestContainerFixture : IAsyncLifetime
{
    private PostgreSqlTestcontainer _container = default!;
    public string ConnectionString => _container.ConnectionString;

    public async Task InitializeAsync()
    {
        _container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
            .WithDatabase(new PostgreSqlTestcontainerConfiguration
            {
                Database = "testdb",
                Username = "postgres",
                Password = "postgres"
            })
            .WithImage("postgres:15")
            .WithCleanUp(true)
            .Build();

        await _container.StartAsync();

        await EnsureDatabaseIsReady();
        await ApplyMigrations();

        await ResetDatabaseAsync();
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }

    private async Task EnsureDatabaseIsReady()
    {
        // Wait for database to be connectable
        var options = new DbContextOptionsBuilder<ContosoPizzaDbContext>()
            .UseNpgsql(_container.ConnectionString)
            .Options;

        for (int i = 0; i < 10; i++)
        {
            try
            {
                using var db = new ContosoPizzaDbContext(options);
                await db.Database.OpenConnectionAsync();
                await db.Database.CloseConnectionAsync();
                return;
            }
            catch
            {
                await Task.Delay(1000);
            }
        }

        throw new Exception("PostgreSQL container not ready.");
    }

    private async Task ApplyMigrations()
    {
        var options = new DbContextOptionsBuilder<ContosoPizzaDbContext>()
            .UseNpgsql(_container.ConnectionString)
            .Options;

        using var db = new ContosoPizzaDbContext(options);
        await db.Database.MigrateAsync();
    }

    // Provide clean DbContext to tests
    public ContosoPizzaDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ContosoPizzaDbContext>()
            .UseNpgsql(_container.ConnectionString)
            .Options;

        return new ContosoPizzaDbContext(options);
    }

    public async Task ResetDatabaseAsync()
    {
        await using var conn = new Npgsql.NpgsqlConnection(ConnectionString);
        await conn.OpenAsync();

        // Ge all public schemas
        var getTablesCmd = conn.CreateCommand();
        getTablesCmd.CommandText = @"
            SELECT tablename
            FROM pg_tables
            WHERE schemaname = 'public';
        ";

        var tableNames = new List<string>();
        await using (var reader = await getTablesCmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                tableNames.Add(reader.GetString(0));
            }
        }

        if (tableNames.Count == 0)
            return;

        // Build and run a TRUNCATE command to truncate all data
        var truncateCmd = conn.CreateCommand();
        truncateCmd.CommandText = $"TRUNCATE TABLE {string.Join(", ", tableNames.Select(n => $"\"{n}\""))} RESTART IDENTITY CASCADE;";
        await truncateCmd.ExecuteNonQueryAsync();
    }
}
