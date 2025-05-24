using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MovieBookingSystem.Infrastructure.Data;
using System;
using System.Linq;

namespace MovieBookingSystem.Tests.Integration
{
    public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's ApplicationDbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<MovieBookingDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add ApplicationDbContext using an in-memory database for testing
                services.AddDbContext<MovieBookingDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb_" + Guid.NewGuid().ToString());
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                });

                // Enable logging for debugging
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddDebug();
                    builder.SetMinimumLevel(LogLevel.Debug);
                });

                // Build the service provider
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database context
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<MovieBookingDbContext>();

                    try
                    {
                        // Ensure the database is created
                        db.Database.EnsureCreated();
                    }
                    catch (Exception ex)
                    {
                        // Log errors or do nothing if you want to ignore setup issues
                        var logger = scopedServices.GetRequiredService<ILogger<TestWebApplicationFactory<TStartup>>>();
                        logger.LogError(ex, "An error occurred creating the test database.");
                    }
                }
            });

            builder.UseEnvironment("Testing");
        }
    }
}
