using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestTube.Data;
using TestTube.DTOs;
using TestTube.Models;

namespace TestTube.Tests;

public static class TestHelper
{
    private static readonly HttpClient _client = new HttpClient
    {
        BaseAddress = new Uri("http://localhost:5081")
    };

    public static ApplicationDbContext CreateInMemoryDbContext()
    {
        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestTubeInMemoryDb_{Guid.NewGuid()}")
            .UseInternalServiceProvider(serviceProvider)
            .Options;

        var dbContext = new ApplicationDbContext(options);
        SeedDatabase(dbContext);
        return dbContext;
    }

    public static void SeedDatabase(ApplicationDbContext dbContext)
    {
        // Add scientists
        var scientist1 = new Scientist
        {
            Id = 1,
            Name = "Marie Curie",
            Department = "Physics",
            Email = "marie.curie@testtube.com",
            HireDate = new DateTime(2020, 1, 15)
        };

        var scientist2 = new Scientist
        {
            Id = 2,
            Name = "Albert Einstein",
            Department = "Physics",
            Email = "albert.einstein@testtube.com",
            HireDate = new DateTime(2021, 3, 10)
        };

        var scientist3 = new Scientist
        {
            Id = 3,
            Name = "Rosalind Franklin",
            Department = "Chemistry",
            Email = "rosalind.franklin@testtube.com",
            HireDate = new DateTime(2022, 5, 20)
        };

        dbContext.Scientists.AddRange(scientist1, scientist2, scientist3);
        dbContext.SaveChanges();

        // Add equipment
        var equipment1 = new Equipment
        {
            Id = 1,
            Name = "Microscope",
            SerialNumber = "MS-12345",
            Manufacturer = "Zeiss",
            PurchaseDate = new DateTime(2021, 2, 10),
            Price = 15000.00m,
            ScientistId = 1
        };

        var equipment2 = new Equipment
        {
            Id = 2,
            Name = "Centrifuge",
            SerialNumber = "CF-67890",
            Manufacturer = "Thermo Fisher",
            PurchaseDate = new DateTime(2022, 4, 15),
            Price = 8500.00m,
            ScientistId = 1
        };

        var equipment3 = new Equipment
        {
            Id = 3,
            Name = "Spectrometer",
            SerialNumber = "SP-24680",
            Manufacturer = "Agilent",
            PurchaseDate = new DateTime(2023, 1, 5),
            Price = 22000.00m,
            ScientistId = 2
        };

        var equipment4 = new Equipment
        {
            Id = 4,
            Name = "PCR Machine",
            SerialNumber = "PCR-13579",
            Manufacturer = "Bio-Rad",
            PurchaseDate = new DateTime(2023, 6, 20),
            Price = 12000.00m,
            ScientistId = 3
        };

        dbContext.Equipment.AddRange(equipment1, equipment2, equipment3, equipment4);
        dbContext.SaveChanges();
    }

    // HTTP client methods for integration testing
    public static async Task<List<ScientistDto>> GetAllScientistsAsync()
    {
        var response = await _client.GetAsync("/scientists");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<ScientistDto>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public static async Task<ScientistDetailDto> GetScientistByIdAsync(int id)
    {
        var response = await _client.GetAsync($"/scientists/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ScientistDetailDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public static async Task<List<EquipmentDto>> GetAllEquipmentAsync()
    {
        var response = await _client.GetAsync("/equipment");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<EquipmentDto>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public static async Task<EquipmentDetailDto> GetEquipmentByIdAsync(int id)
    {
        var response = await _client.GetAsync($"/equipment/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<EquipmentDetailDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}