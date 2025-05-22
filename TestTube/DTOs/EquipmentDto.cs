namespace TestTube.DTOs;

using System;
using System.Text.Json.Serialization;

public class EquipmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;

    [JsonConverter(typeof(DateTimeJsonConverter))]
    public DateTime PurchaseDate { get; set; }
    public decimal Price { get; set; }
    public int ScientistId { get; set; }
}

public class EquipmentDetailDto : EquipmentDto
{
    public ScientistDto? Scientist { get; set; }
}