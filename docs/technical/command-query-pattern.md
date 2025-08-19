# Command and Query Pattern Implementation

## Overview

This document describes the implementation of the Command and Query pattern in the TaskManager application. The pattern is implemented using FastEndpoints, which provides a lightweight and efficient way to handle HTTP requests.

## Architecture

The application follows a Clean Architecture approach, with the following layers:

- **Domain**: Contains the core business logic and entities.
- **Application**: Contains the use cases (commands and queries) and interfaces for external services.
- **Infrastructure**: Contains the implementation of external services (e.g., database access).
- **WebApi**: Contains the HTTP endpoints that handle incoming requests.

## Commands and Queries

### Commands

Commands represent actions that change the state of the system. They are implemented as records that inherit from `ICommand<TResponse>`.

Example:

```csharp
public record CreateProjectCommand(string Name, string? Description) : ICommand<ProjectResponse>;
```

### Queries

Queries represent requests for data that do not change the state of the system. They are implemented as records that inherit from `IQuery<TResponse>`.

Example:

```csharp
public record GetProjectsQuery(bool Dummy = false) : IQuery<IEnumerable<ProjectResponse>>;
```

## Handlers

Handlers are responsible for executing commands and queries. They are implemented as endpoints in the WebApi layer.

### Command Handlers

Command handlers are implemented as endpoints that inherit from `Endpoint<TCommand, TResponse, TMapper>`.

Example:

```csharp
public class CreateProjectEndpoint
    : Endpoint<CreateProjectCommand, ProjectResponse, CreateProjectMapper>
{
    private readonly KanboardDbContext _dbContext;

    public CreateProjectEndpoint(KanboardDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/projects");
        AllowAnonymous(); // Temporaneo, da rimuovere con l'autenticazione
        Summary(s =>
        {
            s.Summary = "Crea un nuovo progetto";
            s.Description = "Crea un nuovo progetto TaskManager con nome e descrizione.";
            s.ExampleRequest = new CreateProjectCommand("My New Project", "A description for my new project.");
            s.ResponseExamples[201] = new ProjectResponse(Guid.NewGuid(), "My New Project", "A description for my new project.", DateTime.UtcNow);
        });
    }

    public override async Task HandleAsync(CreateProjectCommand req, CancellationToken ct)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = req.Name,
            Description = req.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync(ct);

        var response = Map.FromEntity(project);

        await HttpContext.Response.WriteAsJsonAsync(response, ct);
    }
}
```

### Query Handlers

Query handlers are implemented similarly to command handlers.

Example:

```csharp
public class GetProjectsEndpoint
    : Endpoint<GetProjectsQuery, IEnumerable<ProjectResponse>, GetProjectsMapper>
{
    private readonly KanboardDbContext _dbContext;

    public GetProjectsEndpoint(KanboardDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/projects");
        AllowAnonymous(); // Temporaneo, da rimuovere con l'autenticazione
        Summary(s =>
        {
            s.Summary = "Ottieni tutti i progetti";
            s.Description = "Ottieni un elenco di tutti i progetti TaskManager.";
            s.ResponseExamples[200] = new[]
            {
                new ProjectResponse(Guid.NewGuid(), "Project 1", "Description 1", DateTime.UtcNow)
            };
        });
    }

    public override async Task HandleAsync(GetProjectsQuery req, CancellationToken ct)
    {
        try
        {
            var projects = await _dbContext.Projects.ToListAsync(ct);
            var response = Map.FromEntity(projects);

            HttpContext.Response.StatusCode = 200;
            await HttpContext.Response.WriteAsJsonAsync(response, ct);
        }
        catch (Exception ex)
        {
            // Log the exception for debugging
            Console.WriteLine($"GetProjectsEndpoint Exception: {ex}");
            throw;
        }
    }
}
```

## Mappers

Mappers are responsible for converting between domain entities and DTOs (Data Transfer Objects). They are implemented using the Mapperly library, which generates efficient mapping code at compile time.

### Command Mappers

Command mappers inherit from `Mapper<TCommand, TResponse, TEntity>`.

Example:

```csharp
[Mapper]
public partial class CreateProjectMapper : Mapper<CreateProjectCommand, ProjectResponse, Project>
{
    public partial ProjectResponse ToResponse(Project project);

    public override ProjectResponse FromEntity(Project project)
    {
        return new ProjectResponse(project.Id, project.Name, project.Description, project.CreatedAt);
    }
}
```

### Query Mappers

Query mappers are implemented similarly to command mappers.

Example:

```csharp
[Mapper]
public partial class GetProjectsMapper
    : Mapper<GetProjectsQuery, IEnumerable<ProjectResponse>, IEnumerable<Project>>
{
    public partial IEnumerable<ProjectResponse> ToResponse(IEnumerable<Project> projects);

    public IEnumerable<ProjectResponse> FromEntity(IEnumerable<Project> projects)
    {
        return ToResponse(projects);
    }
}
```

## Validation

Validation is handled using FluentValidation. Validators are implemented as classes that inherit from `Validator<TCommand>`.

Example:

```csharp
public class CreateProjectCommandValidator : Validator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Il nome del progetto è obbligatorio.")
            .MaximumLength(100).WithMessage("Il nome del progetto non può superare i 100 caratteri.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descrizione del progetto non può superare i 500 caratteri.");
    }
}
```

## Dependency Injection

Dependencies are injected using the built-in ASP.NET Core DI container. Services are registered in the `Program.cs` file.

Example:

```csharp
builder.Services.AddDbContext<KanboardDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<KanboardDbContext>());
```

## Testing

Testing is performed using xUnit. Integration tests are implemented using `WebApplicationFactory` to create a test server.

Example:

```csharp
public class FastEndpointsTestBase : WebApplicationFactory<Startup>
{
    private readonly string _databaseName = $"TestDb_{Guid.NewGuid():N}";

    /// <summary>
    /// Gets the HTTP client for making requests to the test server.
    /// </summary>
    public HttpClient HttpClient { get; private set; } = default!;

    /// <inheritdoc />
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace the database with an in-memory SQLite database for testing
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<KanboardDbContext>));

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
    public async Task InitializeAsync()
    {
        HttpClient = CreateClient();

        // Ensure the database is created
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<KanboardDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }

    /// <inheritdoc />
    public async Task DisposeAsync()
    {
        HttpClient?.Dispose();

        // Clean up the in-memory database
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<KanboardDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
    }
}
```

## Conclusion

The Command and Query pattern implemented in TaskManager provides a clean separation of concerns, making the codebase more maintainable and testable. The use of FastEndpoints and Mapperly ensures efficient request handling and data mapping.
