using Microsoft.Extensions.DependencyInjection;

namespace TestTube.Tests;

public class ScientistTests : IClassFixture<TestWebApplicationFactory>, IDisposable
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ScientistTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAllScientists_ReturnsAllScientists()
    {
        // Implement the test to get all scientists. Use the code in EquipmentTests as a reference.

        Assert.Equal(1, 2); // Remove this line before implementing the test
    }

    [Fact]
    public async Task GetScientistById_ReturnsCorrectScientist()
    {
        // Arrange
        int scientistId = 1;

        // Act
        var scientist = await TestHelper.GetScientistByIdAsync(_client, scientistId);

        // Assert
        Assert.NotNull(scientist);
        Assert.Equal(scientistId, scientist.Id);
        Assert.Equal("Marie Curie", scientist.Name);
        Assert.Equal("Physics", scientist.Department);
        Assert.NotNull(scientist.Equipment);
        // We don't check for a specific count as the number of equipment items can vary

        // Check equipment details
        var equipment = scientist.Equipment.ToList();
        Assert.Contains(equipment, e => e.Name == "Microscope");
        Assert.Contains(equipment, e => e.Name == "Centrifuge");
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}