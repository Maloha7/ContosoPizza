using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Data;

public class PizzaDb : DbContext
{
    public PizzaDb(DbContextOptions<PizzaDb> options) : base(options) { }

    public DbSet<Pizza> Pizzas { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pizza>().HasData(
            new Pizza { Id = 1, Name = "Margherita", IsGlutenFree = false },
            new Pizza { Id = 2, Name = "Pepperoni", IsGlutenFree = false },
            new Pizza { Id = 3, Name = "Gluten-Free Veggie", IsGlutenFree = false },
            new Pizza { Id = 4, Name = "Hawaiian", IsGlutenFree = false },
            new Pizza { Id = 5, Name = "Diavola", IsGlutenFree = false },
            new Pizza { Id = 6, Name = "Quattro Formaggi", IsGlutenFree = false },
            new Pizza { Id = 7, Name = "Caprese", IsGlutenFree = false },
            new Pizza { Id = 8, Name = "Napoli", IsGlutenFree = false }
        );
    }
}
