using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskManager.Infrastructure.Persistence;
using TaskManager.WebApi;
using Xunit;

namespace TaskManager.IntegrationTests;

/// <summary>
/// Base class for FastEndpoints integration tests.
/// </summary>
public class FastEndpointsTestBase : WebApplicationFactory<TaskManager.WebApi.Startup>
{
    private readonly string _databaseName = $"TestDb_{Guid.NewGuid():N}";

    /// <summary>
    /// Gets the HTTP client for making requests to the test server.
    /// </summary>
    public HttpClient HttpClient { get; private set; } = default!;

    /// <summary>
    /// Creates and configures an HTTP client for integration tests.
    /// </summary>
    /// <returns>The configured HTTP client.</returns>
    public async Task<HttpClient> GetHttpClientAsync()
    {
        if (HttpClient == null)
        {
            HttpClient = CreateClient();

            // Ensure the database is created
            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<KanboardDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
        }

        return HttpClient;
    }

    /// <inheritdoc />
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace the database with an in-memory SQLite database for testing
            var dbContextDescriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<KanboardDbContext>)
            );

            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            services.AddDbContext<KanboardDbContext>(options =>
            {
                options.UseSqlite($"Filename={_databaseName}");
            });
        });

        return base.CreateHost(builder);
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        HttpClient?.Dispose();

        // Clean up the in-memory database
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<KanboardDbContext>();
        await dbContext.Database.EnsureDeletedAsync();

        await base.DisposeAsync();
    }
}
