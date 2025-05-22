using TestTube.DTOs;
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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
        Assert.IsType<List<ScientistDto>>(scientists);
        Assert.True(scientists.Count > 0, "Should return at least one scientist");

        // Verify structure of returned scientists
        foreach (var scientist in scientists)
        {
            Assert.True(scientist.Id > 0, "Scientist should have a valid ID");
            Assert.NotNull(scientist.Name);
            Assert.NotEmpty(scientist.Name);
            Assert.NotNull(scientist.Department);
            Assert.NotEmpty(scientist.Department);
            Assert.NotNull(scientist.Email);
            Assert.NotEmpty(scientist.Email);
            Assert.True(scientist.HireDate > DateTime.MinValue, "Scientist should have a valid hire date");
        }
    }

    [Fact]
    public async Task GetScientistById_ReturnsCorrectScientist()
    {
        // Arrange - First get all scientists to find a valid ID
        var allScientists = await TestHelper.GetAllScientistsAsync();
        Assert.True(allScientists.Count > 0, "Test requires at least one scientist in database");

        // Find a scientist that has equipment (if possible)
        var scientistWithEquipment = allScientists.FirstOrDefault();
        int scientistId = scientistWithEquipment.Id;

        // Act
        var scientist = await TestHelper.GetScientistByIdAsync(scientistId);

        // Assert
        Assert.NotNull(scientist);
        Assert.Equal(scientistId, scientist.Id);
        Assert.NotEmpty(scientist.Name);
        Assert.NotEmpty(scientist.Department);
        Assert.NotNull(scientist.Equipment);

        // If equipment exists, verify its structure
        if (scientist.Equipment.Count > 0)
        {
            foreach (var item in scientist.Equipment)
            {
                Assert.True(item.Id > 0);
                Assert.NotEmpty(item.Name);
                Assert.NotEmpty(item.SerialNumber);
                Assert.NotEmpty(item.Manufacturer);
            }
        }
    }

    [Fact]
    public async Task CreateScientist_ReturnsCreatedScientist()
    {
        // Arrange
        string uniqueName = $"Test Scientist {Guid.NewGuid()}";
        var newScientist = new ScientistDto
        {
            Name = uniqueName,
            Department = "Test Department",
            Email = $"test.{Guid.NewGuid().ToString().Substring(0, 8)}@testtube.com",
            HireDate = DateTime.Now.AddMonths(-3)
        };

        // Act
        var createdScientist = await TestHelper.CreateScientistAsync(newScientist);

        // Assert
        Assert.NotNull(createdScientist);
        Assert.True(createdScientist.Id > 0); // Should have an ID assigned
        Assert.Equal(newScientist.Name, createdScientist.Name);
        Assert.Equal(newScientist.Department, createdScientist.Department);
        Assert.Equal(newScientist.Email, createdScientist.Email);
        Assert.Equal(newScientist.HireDate, createdScientist.HireDate);

        // Verify the scientist was actually created in the database
        var allScientists = await TestHelper.GetAllScientistsAsync();
        Assert.Contains(allScientists, s => s.Id == createdScientist.Id);
    }

    [Fact]
    public async Task UpdateScientist_ReturnsUpdatedScientist()
    {
        // Arrange - First get all scientists to find a valid ID
        var allScientists = await TestHelper.GetAllScientistsAsync();
        Assert.True(allScientists.Count > 0, "Test requires at least one scientist in database");

        var scientistToUpdate = allScientists.First();
        int scientistId = scientistToUpdate.Id;

        var updatedScientist = new ScientistDto
        {
            Id = scientistId,
            Name = $"Updated Scientist {Guid.NewGuid()}",
            Department = "Updated Department",
            Email = $"updated.{Guid.NewGuid().ToString().Substring(0, 8)}@testtube.com",
            HireDate = DateTime.Now.AddYears(-1)
        };

        // Act
        var result = await TestHelper.UpdateScientistAsync(scientistId, updatedScientist);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(scientistId, result.Id);
        Assert.Equal(updatedScientist.Name, result.Name);
        Assert.Equal(updatedScientist.Department, result.Department);
        Assert.Equal(updatedScientist.Email, result.Email);
        Assert.Equal(updatedScientist.HireDate, result.HireDate);

        // Verify the scientist was actually updated in the database
        var scientist = await TestHelper.GetScientistByIdAsync(scientistId);
        Assert.Equal(updatedScientist.Department, scientist.Department);
    }

    [Fact]
    public async Task DeleteScientist_WithNoEquipment_ReturnsNoContent()
    {
        // Arrange - Create a scientist with no equipment to delete
        var newScientist = new ScientistDto
        {
            Name = $"Temporary Scientist {Guid.NewGuid()}",
            Department = "Temporary Department",
            Email = $"temp.{Guid.NewGuid().ToString().Substring(0, 8)}@testtube.com",
            HireDate = DateTime.Now
        };
        var createdScientist = await TestHelper.CreateScientistAsync(newScientist);
        int scientistId = createdScientist.Id;

        // Act
        var response = await TestHelper.DeleteScientistAsync(scientistId);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);

        // Verify the scientist was actually deleted
        var allScientists = await TestHelper.GetAllScientistsAsync();
        Assert.DoesNotContain(allScientists, s => s.Id == scientistId);
    }

    [Fact]
    public async Task DeleteScientist_WithEquipment_ReturnsBadRequest()
    {
        // Arrange - Find a scientist with equipment
        var allScientists = await TestHelper.GetAllScientistsAsync();
        Assert.True(allScientists.Count > 0, "Test requires at least one scientist in database");

        // Find a scientist that has equipment
        int scientistId = -1;
        foreach (var scientist in allScientists)
        {
            var scientistDetail = await TestHelper.GetScientistByIdAsync(scientist.Id);
            if (scientistDetail.Equipment.Count > 0)
            {
                scientistId = scientist.Id;
                break;
            }
        }

        // If no scientist with equipment is found, create one with equipment for testing
        if (scientistId == -1)
        {
            // Create a new scientist
            var newScientist = new ScientistDto
            {
                Name = $"Test Scientist {Guid.NewGuid()}",
                Department = "Test Department",
                Email = $"test.{Guid.NewGuid().ToString().Substring(0, 8)}@testtube.com",
                HireDate = DateTime.Now
            };
            var createdScientist = await TestHelper.CreateScientistAsync(newScientist);
            scientistId = createdScientist.Id;

            // Create equipment for this scientist
            var newEquipment = new EquipmentDto
            {
                Name = $"Test Equipment {Guid.NewGuid()}",
                SerialNumber = $"SN-{Guid.NewGuid().ToString().Substring(0, 8)}",
                Manufacturer = "Test Manufacturer",
                PurchaseDate = DateTime.Now,
                Price = 1000.00m,
                ScientistId = scientistId
            };
            await TestHelper.CreateEquipmentAsync(newEquipment);
        }

        // Act
        var response = await TestHelper.DeleteScientistAsync(scientistId);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

        // Verify the scientist was not deleted
        var updatedScientists = await TestHelper.GetAllScientistsAsync();
        Assert.Contains(updatedScientists, s => s.Id == scientistId);
    }

    [Fact]
    public async Task DeleteScientist_NonExistent_ReturnsNotFound()
    {
        // Arrange - Find a non-existent scientist ID
        var allScientists = await TestHelper.GetAllScientistsAsync();
        int maxId = allScientists.Count > 0 ? allScientists.Max(s => s.Id) : 0;
        int nonExistentId = maxId + 1000; // Ensure it's well beyond any existing ID

        // Act
        var response = await TestHelper.DeleteScientistAsync(nonExistentId);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}