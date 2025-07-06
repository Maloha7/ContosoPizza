using ContoroPizza.Dtos;
using ContoroPizza.Interfaces;

using ContosoPizza.Models;

using Microsoft.AspNetCore.Mvc;

namespace ContoroPizza.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class PizzaController(IPizzaService pizzaService) : ControllerBase
{
    /// <summary>
    /// Get all available pizzas
    /// </summary>
    /// <returns> A list of all available Pizzas </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Pizza>>> GetAll()
    {
        List<Pizza> pizzas = await pizzaService.GetAll();
        return Ok(pizzas);
    }

    /// <summary>
    /// Get a specific pizza
    /// </summary>
    /// <param name="id">The id of the pizza</param>
    /// <returns>The pizza with the specified id</returns>
    /// <response code="200">Returns the pizza with the specified id if it exists</response>
    /// <response code="404">Returns Not Found if the pizza with the specified id does not exist</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Pizza>> Get(int id)
    {
        var pizza = await pizzaService.Get(id);
        if (pizza == null)
            return NotFound();

        return Ok(pizza);
    }


    /// <summary>
    /// Add a new pizza
    /// </summary>
    /// <returns>A newly created pizza</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /pizza
    ///     {
    ///         "name": "Hawaii",
    ///         "isGlutenFree": false
    ///     }
    /// </remarks>
    /// <param name="pizza">The pizza that should be created</param>
    /// <response code="201">Returns status code 201 created</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(AddPizzaDto pizza)
    {
        var createdPizza = await pizzaService.Add(pizza.toPizza());
        return CreatedAtAction(nameof(Get), new { id = createdPizza.Id }, pizza);
    }

    /// <summary>
    /// Update a pizza
    /// </summary>
    /// <param name="id">The id of the pizza to update</param>
    /// <param name="pizza"> The updated pizza</param>
    /// <response code="400">If the id's do not match</response>
    /// <response code="404">If the id does not correspond to an existing pizza</response>
    /// <response code="204">If the pizza was updated successfully</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(int id, Pizza pizza)
    {
        if (id != pizza.Id)
            return BadRequest();

        var existingPizza = await pizzaService.Get(id);
        if (existingPizza == null)
            return NotFound();

        await pizzaService.Update(id, pizza);

        return NoContent();
    }

    /// <summary>
    /// Delete a pizza
    /// </summary>
    /// <param name="id">The id of the pizza to delete</param>
    /// <response code="404">If the id does not correspond to an existing pizza</response>
    /// <response code="204">If the pizza was deleted successfully</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        var existingPizza = await pizzaService.Get(id);
        if (existingPizza is null)
            return NotFound();

        await pizzaService.Delete(id);

        return NoContent();

    }
}
