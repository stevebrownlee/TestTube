using Microsoft.EntityFrameworkCore;
using TestTube.Data;
using TestTube.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Configure database based on environment
if (builder.Environment.IsEnvironment("Testing"))
{
    // For testing, use in-memory database
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("TestTubeInMemoryDb"));
}
else
{
    // For development and production, use PostgreSQL
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}

var app = builder.Build();

// Define API endpoints
app.MapGet("/", () => "Welcome to TestTube API!");

// Map endpoints from endpoint classes
app.MapScientistEndpoints();
app.MapEquipmentEndpoints();

app.Run();

// Make Program class public for testing
public partial class Program { }
