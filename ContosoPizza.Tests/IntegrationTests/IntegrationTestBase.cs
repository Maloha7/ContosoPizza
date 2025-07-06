using ContosoPizza.Data;

[Collection("PostgresTestCollection")]
public abstract class IntegrationTestBase : IAsyncLifetime
{
    protected readonly PostgresTestContainerFixture Fixture;

    protected IntegrationTestBase(PostgresTestContainerFixture fixture)
    {
        Fixture = fixture;
    }

    protected PizzaDb CreateDbContext() => Fixture.CreateContext();

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        // Clean up DB after every test
        await Fixture.ResetDatabaseAsync();
    }
}
