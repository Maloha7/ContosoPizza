using ContoroPizza.Interfaces;

using ContosoPizza.Data;
using ContosoPizza.Models;

using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;

public class PizzaService(PizzaDb db) : IPizzaService
{
    public async Task<Pizza> Add(Pizza pizza)
    {
        var createdPizza = await db.Pizzas.AddAsync(pizza);
        await db.SaveChangesAsync();

        return createdPizza.Entity;
    }

    public async Task Delete(int id)
    {
        var pizza = await db.Pizzas.FindAsync(id);

        db.Pizzas.Remove(pizza!);
        await db.SaveChangesAsync();
    }

    public async Task<Pizza?> Get(int id)
    {
        Pizza? pizza = await db.Pizzas.FindAsync(id);
        return pizza;
    }

    public async Task<List<Pizza>> GetAll()
    {
        List<Pizza> pizzas = await db.Pizzas.ToListAsync();
        return pizzas;
    }

    public async Task<Boolean> Update(int id, Pizza updatedPizza)
    {
        var existingPizza = await db.Pizzas.FindAsync(id);
        if (existingPizza is null)
        {
            return false;
        }

        existingPizza.Name = updatedPizza.Name;
        existingPizza.IsGlutenFree = updatedPizza.IsGlutenFree;
        await db.SaveChangesAsync();

        return true;
    }
}
