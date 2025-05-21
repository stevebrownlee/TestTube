namespace TestTube.Tests;

public class EquipmentTests
{
    [Fact]
    public async Task GetAllEquipment_ReturnsAllEquipment()
    {
        // Act
        var equipment = await TestHelper.GetAllEquipmentAsync();

        // Assert
        Assert.NotNull(equipment);
        Assert.Equal(4, equipment.Count); // We expect 4 pieces of equipment
        Assert.Contains(equipment, e => e.Name == "Microscope");
        Assert.Contains(equipment, e => e.Name == "Centrifuge");
        Assert.Contains(equipment, e => e.Name == "Spectrometer");
        Assert.Contains(equipment, e => e.Name == "PCR Machine");
    }

    [Fact]
    public async Task GetEquipmentById_ReturnsCorrectEquipment()
    {
        // Arrange
        int equipmentId = 1;

        // Act
        var equipment = await TestHelper.GetEquipmentByIdAsync(equipmentId);

        // Assert
        Assert.NotNull(equipment);
        Assert.Equal(equipmentId, equipment.Id);
        Assert.Equal("Microscope", equipment.Name);
        Assert.Equal("MS-12345", equipment.SerialNumber);
        Assert.Equal("Zeiss", equipment.Manufacturer);
        Assert.Equal(1, equipment.ScientistId);

        // Check scientist details
        Assert.NotNull(equipment.Scientist);
        Assert.Equal("Marie Curie", equipment.Scientist.Name);
        Assert.Equal("Physics", equipment.Scientist.Department);
    }
}