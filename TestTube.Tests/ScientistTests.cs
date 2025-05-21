namespace TestTube.Tests;

public class ScientistTests
{
    [Fact]
    public async Task GetAllScientists_ReturnsAllScientists()
    {
        // Act
        var scientists = await TestHelper.GetAllScientistsAsync();

        // Assert
        Assert.NotNull(scientists);
        Assert.Equal(3, scientists.Count); // We expect 3 scientists
        Assert.Contains(scientists, s => s.Name == "Marie Curie");
        Assert.Contains(scientists, s => s.Name == "Albert Einstein");
        Assert.Contains(scientists, s => s.Name == "Rosalind Franklin");
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