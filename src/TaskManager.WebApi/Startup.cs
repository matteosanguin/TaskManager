namespace TaskManager.WebApi;

/// <summary>
/// Startup class for testing purposes.
/// </summary>
public class Startup
{
    /// <summary>
    /// Configures services for the application.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        // This method is intentionally left blank for testing purposes.
        // Services are configured in the main Program.cs file.
    }

    /// <summary>
    /// Configures the application pipeline.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="env">The hosting environment.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // This method is intentionally left blank for testing purposes.
        // The pipeline is configured in the main Program.cs file.
    }
}
