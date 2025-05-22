using System;
using Microsoft.EntityFrameworkCore;
using TestTube.Data;
using TestTube.DTOs;
using TestTube.Models;

namespace TestTube.Endpoints
{
    public static class EquipmentEndpoints
    {
        public static void MapEquipmentEndpoints(this WebApplication app)
        {
            // GET /equipment - Get all equipment
            app.MapGet("/equipment", async (ApplicationDbContext db) =>
            {
                var equipment = await db.Equipment.Select(e => new EquipmentDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    SerialNumber = e.SerialNumber,
                    Manufacturer = e.Manufacturer,
                    PurchaseDate = e.PurchaseDate,
                    Price = e.Price,
                    ScientistId = e.ScientistId
                }).ToListAsync();

                return Results.Ok(equipment);
            });

            // GET /equipment/{id} - Get equipment by ID
            app.MapGet("/equipment/{id}", async (int id, ApplicationDbContext db) =>
            {
                var equipment = await db.Equipment
                    .Include(e => e.Scientist)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (equipment == null)
                    return Results.NotFound();

                var equipmentDto = new EquipmentDetailDto
                {
                    Id = equipment.Id,
                    Name = equipment.Name,
                    SerialNumber = equipment.SerialNumber,
                    Manufacturer = equipment.Manufacturer,
                    PurchaseDate = equipment.PurchaseDate,
                    Price = equipment.Price,
                    ScientistId = equipment.ScientistId,
                    Scientist = equipment.Scientist != null ? new ScientistDto
                    {
                        Id = equipment.Scientist.Id,
                        Name = equipment.Scientist.Name,
                        Department = equipment.Scientist.Department,
                        Email = equipment.Scientist.Email,
                        HireDate = equipment.Scientist.HireDate
                    } : null
                };

                return Results.Ok(equipmentDto);
            });

            // POST /equipment - Create a new equipment
            app.MapPost("/equipment", async (EquipmentDto equipmentDto, ApplicationDbContext db) =>
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(equipmentDto.Name) ||
                    string.IsNullOrWhiteSpace(equipmentDto.SerialNumber) ||
                    string.IsNullOrWhiteSpace(equipmentDto.Manufacturer))
                {
                    return Results.BadRequest("Name, SerialNumber, and Manufacturer are required fields");
                }

                // Verify scientist exists
                var scientist = await db.Scientists.FindAsync(equipmentDto.ScientistId);
                if (scientist == null)
                {
                    return Results.BadRequest($"Scientist with ID {equipmentDto.ScientistId} not found");
                }

                // Create new equipment entity
                var equipment = new Equipment
                {
                    Name = equipmentDto.Name,
                    SerialNumber = equipmentDto.SerialNumber,
                    Manufacturer = equipmentDto.Manufacturer,
                    // Preserve the full DateTime value including time component
                    // Just ensure it's in UTC format for PostgreSQL compatibility
                    PurchaseDate = equipmentDto.PurchaseDate.Kind == DateTimeKind.Utc
                        ? equipmentDto.PurchaseDate
                        : DateTime.SpecifyKind(equipmentDto.PurchaseDate, DateTimeKind.Utc),
                    Price = equipmentDto.Price,
                    ScientistId = equipmentDto.ScientistId
                };

                // Add to database
                db.Equipment.Add(equipment);
                await db.SaveChangesAsync();

                // Return created equipment with ID
                return Results.Created($"/equipment/{equipment.Id}", new EquipmentDto
                {
                    Id = equipment.Id,
                    Name = equipment.Name,
                    SerialNumber = equipment.SerialNumber,
                    Manufacturer = equipment.Manufacturer,
                    PurchaseDate = equipment.PurchaseDate,
                    Price = equipment.Price,
                    ScientistId = equipment.ScientistId
                });
            });

            // PUT /equipment/{id} - Update equipment
            app.MapPut("/equipment/{id}", async (int id, EquipmentDto equipmentDto, ApplicationDbContext db) =>
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(equipmentDto.Name) ||
                    string.IsNullOrWhiteSpace(equipmentDto.SerialNumber) ||
                    string.IsNullOrWhiteSpace(equipmentDto.Manufacturer))
                {
                    return Results.BadRequest("Name, SerialNumber, and Manufacturer are required fields");
                }

                // Find existing equipment
                var equipment = await db.Equipment.FindAsync(id);
                if (equipment == null)
                {
                    return Results.NotFound($"Equipment with ID {id} not found");
                }

                // Verify scientist exists
                var scientist = await db.Scientists.FindAsync(equipmentDto.ScientistId);
                if (scientist == null)
                {
                    return Results.BadRequest($"Scientist with ID {equipmentDto.ScientistId} not found");
                }

                // Update properties
                equipment.Name = equipmentDto.Name;
                equipment.SerialNumber = equipmentDto.SerialNumber;
                equipment.Manufacturer = equipmentDto.Manufacturer;
                // Preserve the full DateTime value including time component
                // Just ensure it's in UTC format for PostgreSQL compatibility
                equipment.PurchaseDate = equipmentDto.PurchaseDate.Kind == DateTimeKind.Utc
                    ? equipmentDto.PurchaseDate
                    : DateTime.SpecifyKind(equipmentDto.PurchaseDate, DateTimeKind.Utc);
                equipment.Price = equipmentDto.Price;
                equipment.ScientistId = equipmentDto.ScientistId;

                // Save changes
                await db.SaveChangesAsync();

                // Return updated equipment
                return Results.Ok(new EquipmentDto
                {
                    Id = equipment.Id,
                    Name = equipment.Name,
                    SerialNumber = equipment.SerialNumber,
                    Manufacturer = equipment.Manufacturer,
                    PurchaseDate = equipment.PurchaseDate,
                    Price = equipment.Price,
                    ScientistId = equipment.ScientistId
                });
            });

            // DELETE /equipment/{id} - Delete equipment
            app.MapDelete("/equipment/{id}", async (int id, ApplicationDbContext db) =>
            {
                // Find equipment
                var equipment = await db.Equipment.FindAsync(id);
                if (equipment == null)
                {
                    return Results.NotFound($"Equipment with ID {id} not found");
                }

                // Remove equipment
                db.Equipment.Remove(equipment);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}