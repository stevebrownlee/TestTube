using Microsoft.EntityFrameworkCore;
using TestTube.Data;
using TestTube.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Configure PostgreSQL with Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Define API endpoints
app.MapGet("/", () => "Welcome to TestTube API!");

// Map endpoints from endpoint classes
app.MapScientistEndpoints();
app.MapEquipmentEndpoints();

app.Run();

// Make Program class public for testing
public partial class Program { }
