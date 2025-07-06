using ContosoPizza.Models;

namespace ContoroPizza.Interfaces;

public interface IPizzaService
{
    Task<List<Pizza>> GetAll();
    Task<Pizza?> Get(int id);
    Task<Pizza> Add(Pizza pizza);
    Task Delete(int id);
    Task<Boolean> Update(int id, Pizza updatedPizza);
}
