using TestTube.DTOs;
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        // No assumptions about the count - could be 0, could be many

        // If any equipment items are returned, verify they have valid properties
        foreach (var item in equipment)
        {
            Assert.True(item.Id > 0, "Equipment ID should be greater than 0");
            Assert.False(string.IsNullOrEmpty(item.Name), "Equipment Name should not be empty");
            Assert.False(string.IsNullOrEmpty(item.SerialNumber), "Equipment SerialNumber should not be empty");
            Assert.False(string.IsNullOrEmpty(item.Manufacturer), "Equipment Manufacturer should not be empty");
            Assert.True(item.ScientistId > 0, "ScientistId should be greater than 0");
        }
    }

    [Fact]
    public async Task GetEquipmentById_ReturnsCorrectEquipment()
    {
        // Arrange - First get all equipment to find a valid ID
        var allEquipment = await TestHelper.GetAllEquipmentAsync();
        Assert.True(allEquipment.Count > 0, "Test requires at least one equipment item in database");

        var firstEquipment = allEquipment.First();
        int equipmentId = firstEquipment.Id;

        // Act
        var equipment = await TestHelper.GetEquipmentByIdAsync(equipmentId);

        // Assert
        Assert.NotNull(equipment);
        Assert.Equal(equipmentId, equipment.Id);
        Assert.NotEmpty(equipment.Name);
        Assert.NotEmpty(equipment.SerialNumber);
        Assert.NotEmpty(equipment.Manufacturer);
        Assert.True(equipment.PurchaseDate > DateTime.MinValue);
        Assert.True(equipment.Price > 0);
        Assert.True(equipment.ScientistId > 0);

        // Assert scientist details
        Assert.NotNull(equipment.Scientist);
        Assert.True(equipment.Scientist.Id > 0);
        Assert.NotEmpty(equipment.Scientist.Name);
        Assert.NotEmpty(equipment.Scientist.Department);
    }

    [Fact]
    public async Task CreateEquipment_ReturnsCreatedEquipment()
    {
        // Arrange - First get all scientists to find a valid ID
        var allScientists = await TestHelper.GetAllScientistsAsync();
        Assert.True(allScientists.Count > 0, "Test requires at least one scientist in database");

        var scientist = allScientists.First();
        int scientistId = scientist.Id;

        var newEquipment = new EquipmentDto
        {
            Name = $"Test Equipment {Guid.NewGuid()}",
            SerialNumber = $"SN-{Guid.NewGuid().ToString().Substring(0, 8)}",
            Manufacturer = "Test Manufacturer",
            PurchaseDate = DateTime.Now.AddDays(-15),
            Price = 9500.00m,
            ScientistId = scientistId // Use a dynamically found scientist ID
        };

        // Act
        var createdEquipment = await TestHelper.CreateEquipmentAsync(newEquipment);

        // Assert
        Assert.NotNull(createdEquipment);
        Assert.True(createdEquipment.Id > 0); // Should have an ID assigned
        Assert.Equal(newEquipment.Name, createdEquipment.Name);
        Assert.Equal(newEquipment.SerialNumber, createdEquipment.SerialNumber);
        Assert.Equal(newEquipment.Manufacturer, createdEquipment.Manufacturer);
        Assert.Equal(newEquipment.PurchaseDate, createdEquipment.PurchaseDate);
        Assert.Equal(newEquipment.Price, createdEquipment.Price);
        Assert.Equal(newEquipment.ScientistId, createdEquipment.ScientistId);

        // Verify the equipment was actually created in the database
        var allEquipment = await TestHelper.GetAllEquipmentAsync();
        Assert.Contains(allEquipment, e => e.Id == createdEquipment.Id);
    }

    [Fact]
    public async Task UpdateEquipment_ReturnsUpdatedEquipment()
    {
        // Arrange - First get all equipment to find a valid ID
        var allEquipment = await TestHelper.GetAllEquipmentAsync();
        Assert.True(allEquipment.Count > 0, "Test requires at least one equipment item in database");

        var equipmentToUpdate = allEquipment.First();
        int equipmentId = equipmentToUpdate.Id;

        // Get the current scientist ID to maintain the relationship
        var currentEquipment = await TestHelper.GetEquipmentByIdAsync(equipmentId);
        int scientistId = currentEquipment.ScientistId;

        var updatedEquipment = new EquipmentDto
        {
            Id = equipmentId,
            Name = $"Updated Equipment {Guid.NewGuid()}",
            SerialNumber = $"SN-{Guid.NewGuid().ToString().Substring(0, 8)}",
            Manufacturer = "Test Manufacturer Updated",
            PurchaseDate = DateTime.Now.AddDays(-30),
            Price = 9999.99m,
            ScientistId = scientistId // Keep the same scientist
        };

        // Act
        var result = await TestHelper.UpdateEquipmentAsync(equipmentId, updatedEquipment);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(equipmentId, result.Id);
        Assert.Equal(updatedEquipment.Name, result.Name);
        Assert.Equal(updatedEquipment.SerialNumber, result.SerialNumber);
        Assert.Equal(updatedEquipment.Manufacturer, result.Manufacturer);
        Assert.Equal(updatedEquipment.PurchaseDate, result.PurchaseDate);
        Assert.Equal(updatedEquipment.Price, result.Price);
        Assert.Equal(updatedEquipment.ScientistId, result.ScientistId);

        // Verify the equipment was actually updated in the database
        var equipment = await TestHelper.GetEquipmentByIdAsync(equipmentId);
        Assert.Equal(updatedEquipment.Name, equipment.Name);
        Assert.Equal(updatedEquipment.Manufacturer, equipment.Manufacturer);
        Assert.Equal(updatedEquipment.Price, equipment.Price);
    }

    [Fact]
    public async Task DeleteEquipment_ReturnsNoContent()
    {
        // Arrange - First get all scientists to find a valid ID
        var allScientists = await TestHelper.GetAllScientistsAsync();
        Assert.True(allScientists.Count > 0, "Test requires at least one scientist in database");

        var scientist = allScientists.First();
        int scientistId = scientist.Id;

        // Create a new equipment to delete
        var newEquipment = new EquipmentDto
        {
            Name = $"Temporary Equipment {Guid.NewGuid()}",
            SerialNumber = $"TEMP-{Guid.NewGuid().ToString().Substring(0, 8)}",
            Manufacturer = "Test Manufacturer",
            PurchaseDate = DateTime.Now,
            Price = 1000.00m,
            ScientistId = scientistId // Use a dynamically found scientist ID
        };
        var createdEquipment = await TestHelper.CreateEquipmentAsync(newEquipment);
        int equipmentId = createdEquipment.Id;

        // Act
        var response = await TestHelper.DeleteEquipmentAsync(equipmentId);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);

        // Verify the equipment was actually deleted
        var allEquipment = await TestHelper.GetAllEquipmentAsync();
        Assert.DoesNotContain(allEquipment, e => e.Id == equipmentId);
    }

    [Fact]
    public async Task DeleteEquipment_NonExistent_ReturnsNotFound()
    {
        // Arrange - Find a non-existent equipment ID
        var allEquipment = await TestHelper.GetAllEquipmentAsync();
        int maxId = allEquipment.Count > 0 ? allEquipment.Max(e => e.Id) : 0;
        int nonExistentId = maxId + 1000; // Ensure it's well beyond any existing ID

        // Act
        var response = await TestHelper.DeleteEquipmentAsync(nonExistentId);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}