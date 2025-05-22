using System;
using Microsoft.EntityFrameworkCore;
using TestTube.Data;
using TestTube.DTOs;
using TestTube.Models;

namespace TestTube.Endpoints
{
    public static class ScientistEndpoints
    {
        public static void MapScientistEndpoints(this WebApplication app)
        {
            // GET /scientists - Get all scientists
            app.MapGet("/scientists", async (ApplicationDbContext db) =>
            {
                var scientists = await db.Scientists.Select(s => new ScientistDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Department = s.Department,
                    Email = s.Email,
                    HireDate = s.HireDate
                }).ToListAsync();

                return Results.Ok(scientists);
            });

            // GET /scientists/{id} - Get a scientist by ID
            app.MapGet("/scientists/{id}", async (int id, ApplicationDbContext db) =>
            {
                var scientist = await db.Scientists
                    .Include(s => s.Equipment)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (scientist == null)
                    return Results.NotFound();

                var scientistDto = new ScientistDetailDto
                {
                    Id = scientist.Id,
                    Name = scientist.Name,
                    Department = scientist.Department,
                    Email = scientist.Email,
                    HireDate = scientist.HireDate,
                    Equipment = scientist.Equipment.Select(e => new EquipmentDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        SerialNumber = e.SerialNumber,
                        Manufacturer = e.Manufacturer,
                        PurchaseDate = e.PurchaseDate,
                        Price = e.Price,
                        ScientistId = e.ScientistId
                    }).ToList()
                };

                return Results.Ok(scientistDto);
            });

            // POST /scientists - Create a new scientist
            app.MapPost("/scientists", async (ScientistDto scientistDto, ApplicationDbContext db) =>
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(scientistDto.Name) ||
                    string.IsNullOrWhiteSpace(scientistDto.Department) ||
                    string.IsNullOrWhiteSpace(scientistDto.Email))
                {
                    return Results.BadRequest("Name, Department, and Email are required fields");
                }

                // Create new scientist entity
                // The HireDate will be sent as a date string in the request body as "MM/DD/YYYY" and the DTO
                // has it defined as DateTime, so convert the string to DateTime
                // Our custom JsonConverter should have already converted it to UTC
                var scientist = new Scientist
                {
                    Name = scientistDto.Name,
                    Department = scientistDto.Department,
                    Email = scientistDto.Email,
                    // Preserve the full DateTime value including time component
                    // Just ensure it's in UTC format for PostgreSQL compatibility
                    HireDate = scientistDto.HireDate.Kind == DateTimeKind.Utc
                        ? scientistDto.HireDate
                        : DateTime.SpecifyKind(scientistDto.HireDate, DateTimeKind.Utc)
                };

                // Add to database
                db.Scientists.Add(scientist);
                await db.SaveChangesAsync();

                // Return created scientist with ID
                return Results.Created($"/scientists/{scientist.Id}", new ScientistDto
                {
                    Id = scientist.Id,
                    Name = scientist.Name,
                    Department = scientist.Department,
                    Email = scientist.Email,
                    HireDate = scientist.HireDate
                });
            });

            // PUT /scientists - Update a scientist
            app.MapPut("/scientists/{id}", async (int id, ScientistDto scientistDto, ApplicationDbContext db) =>
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(scientistDto.Name) ||
                    string.IsNullOrWhiteSpace(scientistDto.Department) ||
                    string.IsNullOrWhiteSpace(scientistDto.Email))
                {
                    return Results.BadRequest("Name, Department, and Email are required fields");
                }

                // Find existing scientist
                var scientist = await db.Scientists.FindAsync(id);
                if (scientist == null)
                {
                    return Results.NotFound($"Scientist with ID {id} not found");
                }

                // Update properties
                scientist.Name = scientistDto.Name;
                scientist.Department = scientistDto.Department;
                scientist.Email = scientistDto.Email;
                // Preserve the full DateTime value including time component
                // Just ensure it's in UTC format for PostgreSQL compatibility
                scientist.HireDate = scientistDto.HireDate.Kind == DateTimeKind.Utc
                    ? scientistDto.HireDate
                    : DateTime.SpecifyKind(scientistDto.HireDate, DateTimeKind.Utc);

                // Save changes
                await db.SaveChangesAsync();

                // Return updated scientist
                return Results.Ok(new ScientistDto
                {
                    Id = scientist.Id,
                    Name = scientist.Name,
                    Department = scientist.Department,
                    Email = scientist.Email,
                    HireDate = scientist.HireDate
                });
            });

            // DELETE /scientists/{id} - Delete a scientist
            app.MapDelete("/scientists/{id}", async (int id, ApplicationDbContext db) =>
            {
                // Find scientist
                var scientist = await db.Scientists.FindAsync(id);
                if (scientist == null)
                {
                    return Results.NotFound($"Scientist with ID {id} not found");
                }

                // Check if scientist has equipment
                var hasEquipment = await db.Equipment.AnyAsync(e => e.ScientistId == id);
                if (hasEquipment)
                {
                    return Results.BadRequest("Cannot delete scientist with assigned equipment");
                }

                // Remove scientist
                db.Scientists.Remove(scientist);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });
        }
    }
}