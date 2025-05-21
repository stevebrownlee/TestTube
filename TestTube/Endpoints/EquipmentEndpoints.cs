using Microsoft.EntityFrameworkCore;
using TestTube.Data;
using TestTube.DTOs;

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
        }
    }
}