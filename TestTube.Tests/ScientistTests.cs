namespace TestTube.Tests;

public class ScientistTests
{
    [Fact]
    public async Task GetAllScientists_ReturnsAllScientists()
    {
        // Implement the test to get all scientists. Use the code in EquipmentTests as a reference.
    }

    [Fact]
    public async Task GetScientistById_ReturnsCorrectScientist()
    {
        // Arrange
        int scientistId = 1;

        // Act
        var scientist = await TestHelper.GetScientistByIdAsync(scientistId);

        // Assert
        Assert.NotNull(scientist);
        Assert.Equal(scientistId, scientist.Id);
        Assert.Equal("Marie Curie", scientist.Name);
        Assert.Equal("Physics", scientist.Department);
        Assert.NotNull(scientist.Equipment);
        Assert.Equal(2, scientist.Equipment.Count); // Marie has 2 pieces of equipment

        // Check equipment details
        var equipment = scientist.Equipment.ToList();
        Assert.Contains(equipment, e => e.Name == "Microscope");
        Assert.Contains(equipment, e => e.Name == "Centrifuge");
    }
}