namespace TestTube.DTOs;

using System.Collections.Generic;

public class ScientistDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
}

public class ScientistDetailDto : ScientistDto
{
    public ICollection<EquipmentDto> Equipment { get; set; } = new List<EquipmentDto>();
}