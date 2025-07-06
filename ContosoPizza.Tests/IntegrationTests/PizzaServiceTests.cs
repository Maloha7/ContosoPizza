using ContosoPizza.Models;
using ContosoPizza.Services;
using FluentAssertions;

public class PizzaServiceTests : IntegrationTestBase
{
    public PizzaServiceTests(PostgresTestContainerFixture fixture) : base(fixture) { }

    [Fact]
    public async Task CanAddAndRetrievePizza()
    {
        // Arrange
        await using var context = CreateDbContext();
        var service = new PizzaService(context);

        var pizza = new Pizza { Name = "TestPizza", IsGlutenFree = true };

        // Act
        await service.Add(pizza);
        var all = await service.GetAll();

        // Assert
        all.Should().ContainSingle(p => p.Name == "TestPizza");
    }

    [Fact]
    public async Task StartsEmpty()
    {
        var db = CreateDbContext();
        var service = new PizzaService(db);

        var pizzas = await service.GetAll();
        pizzas.Should().BeEmpty(); // passes because DB is reset
    }
}
