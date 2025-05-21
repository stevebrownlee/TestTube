namespace TestTube.Tests;

public class EquipmentTests : IClassFixture<TestWebApplicationFactory>, IDisposable
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public EquipmentTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAllEquipment_ReturnsAllEquipment()
    {
        // Act
        var equipment = await TestHelper.GetAllEquipmentAsync(_client);

        // Assert
        Assert.NotNull(equipment);
        // We don't check for a specific count as the number of items can vary from 0 to n
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

        Assert.Equal(1, 2); // Remove this line before implementing the test

        // Invoke the correct method from TestHelper to get the equipment by ID

        // Assert that the response is not null

        // Assert that the equipment ID matches the expected ID

        // Make an assertion for each property of the equipment

        // Assert that the scientist is not null

        // Assert that all of the scientist's properties are correct
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}