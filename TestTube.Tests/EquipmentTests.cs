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

        // Act
        var equipment = await TestHelper.GetEquipmentByIdAsync(_client, equipmentId);

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

    public void Dispose()
    {
        _client.Dispose();
    }
}