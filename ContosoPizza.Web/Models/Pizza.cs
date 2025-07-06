using System.ComponentModel;

namespace ContosoPizza.Models;

public class Pizza
{
    public int Id { get; set; }
    public string? Name { get; set; }

    [DefaultValue(false)]
    public bool IsGlutenFree { get; set; }

}
