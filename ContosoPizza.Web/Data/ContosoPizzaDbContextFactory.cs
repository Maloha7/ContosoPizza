using ContosoPizza.Web.Helpers;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ContosoPizza.Data;

public class ContosoPizzaDbContextFactory : IDesignTimeDbContextFactory<ContosoPizzaDbContext>
{
    public ContosoPizzaDbContext CreateDbContext(string[] args)
    {
        var configuration = ConfigurationHelper.LoadConfiguration();

        var optionsBuilder = new DbContextOptionsBuilder<ContosoPizzaDbContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

        return new ContosoPizzaDbContext(optionsBuilder.Options);
    }
}
