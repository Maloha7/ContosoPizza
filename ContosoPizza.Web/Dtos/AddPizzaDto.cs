using System.ComponentModel;
using ContosoPizza.Models;

namespace ContoroPizza.Dtos;

public class AddPizzaDto
{
    public string? Name { get; set; }
    [DefaultValue(false)]
    public bool IsGlutenFree { get; set; }

    internal Pizza toPizza()
    {
        return new Pizza { Name = this.Name, IsGlutenFree = this.IsGlutenFree };
    }
}
