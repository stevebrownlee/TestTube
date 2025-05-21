using Microsoft.EntityFrameworkCore;
using TestTube.Data;
using TestTube.DTOs;

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
        }
    }
}