namespace TestTube.Models;

using System;

public class Scientist
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }

    // Navigation property for related equipment
    public ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
}