using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestTube.Data;

namespace TestTube.Tests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove all DbContext and DbContextOptions registrations
            var descriptors = services.Where(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>) ||
                     d.ServiceType == typeof(DbContextOptions) ||
                     d.ServiceType.Name.Contains("DbContextOptions") ||
                     (d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))
            ).ToList();

            foreach (var descriptor in descriptors)
            {
                services.Remove(descriptor);
            }

            // Remove any database provider registrations
            var providerDescriptors = services.Where(
                d => d.ServiceType.Name.Contains("DbContextOptionsExtension") ||
                     d.ServiceType.Name.Contains("PostgreSQL") ||
                     d.ServiceType.Name.Contains("Npgsql")
            ).ToList();

            foreach (var descriptor in providerDescriptors)
            {
                services.Remove(descriptor);
            }

            // Add in-memory database
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase($"TestTubeInMemoryDb_{Guid.NewGuid()}");
            });

            // Create a new service provider for seeding
            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Ensure the database is created and seed it
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            TestHelper.SeedDatabase(db);
        });
    }
}