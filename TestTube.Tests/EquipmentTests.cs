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

        // Invoke the correct method from TestHelper to get the equipment by ID

        // Assert that the response is not null

        // Assert that the equipment ID matches the expected ID

        // Make an assertion for each property of the equipment

        // Assert that the scientist is not null

        // Assert that all of the scientist's properties are correct
    }
}