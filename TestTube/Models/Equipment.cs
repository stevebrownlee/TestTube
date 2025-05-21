namespace TestTube.Models;

public class Equipment
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public decimal Price { get; set; }

    // Foreign key for Scientist
    public int ScientistId { get; set; }

    // Navigation property for the scientist who owns this equipment
    public Scientist? Scientist { get; set; }
}