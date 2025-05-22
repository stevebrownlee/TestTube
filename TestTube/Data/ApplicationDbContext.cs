using Microsoft.EntityFrameworkCore;
using TestTube.Models;

namespace TestTube.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Scientist> Scientists { get; set; } = null!;
    public DbSet<Equipment> Equipment { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure DateTime properties to be stored as timestamp with time zone
        modelBuilder.Entity<Scientist>()
            .Property(s => s.HireDate)
            .HasColumnType("timestamp with time zone");

        modelBuilder.Entity<Equipment>()
            .Property(e => e.PurchaseDate)
            .HasColumnType("timestamp with time zone");

        // Seed data
        modelBuilder.Entity<Scientist>().HasData(
            new Scientist
            {
                Id = 1,
                Name = "Marie Curie",
                Department = "Physics",
                Email = "marie.curie@testtube.com",
                HireDate = new DateTime(2020, 1, 15, 0, 0, 0, DateTimeKind.Utc)
            },
            new Scientist
            {
                Id = 2,
                Name = "Albert Einstein",
                Department = "Physics",
                Email = "albert.einstein@testtube.com",
                HireDate = new DateTime(2021, 3, 10, 0, 0, 0, DateTimeKind.Utc)
            },
            new Scientist
            {
                Id = 3,
                Name = "Rosalind Franklin",
                Department = "Chemistry",
                Email = "rosalind.franklin@testtube.com",
                HireDate = new DateTime(2022, 5, 20, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<Equipment>().HasData(
            new Equipment
            {
                Id = 1,
                Name = "Microscope",
                SerialNumber = "MS-12345",
                Manufacturer = "Zeiss",
                PurchaseDate = new DateTime(2021, 2, 10, 0, 0, 0, DateTimeKind.Utc),
                Price = 15000.00m,
                ScientistId = 1
            },
            new Equipment
            {
                Id = 2,
                Name = "Centrifuge",
                SerialNumber = "CF-67890",
                Manufacturer = "Thermo Fisher",
                PurchaseDate = new DateTime(2022, 4, 15, 0, 0, 0, DateTimeKind.Utc),
                Price = 8500.00m,
                ScientistId = 1
            },
            new Equipment
            {
                Id = 3,
                Name = "Spectrometer",
                SerialNumber = "SP-24680",
                Manufacturer = "Agilent",
                PurchaseDate = new DateTime(2023, 1, 5, 0, 0, 0, DateTimeKind.Utc),
                Price = 22000.00m,
                ScientistId = 2
            },
            new Equipment
            {
                Id = 4,
                Name = "PCR Machine",
                SerialNumber = "PCR-13579",
                Manufacturer = "Bio-Rad",
                PurchaseDate = new DateTime(2023, 6, 20, 0, 0, 0, DateTimeKind.Utc),
                Price = 12000.00m,
                ScientistId = 3
            }
        );
    }
}