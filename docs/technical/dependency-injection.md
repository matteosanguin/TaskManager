# Dependency Injection Configuration

## Overview

This document describes the dependency injection (DI) configuration in the TaskManager application. The application uses the built-in ASP.NET Core DI container to manage the lifecycle of services.

## Services Registration

Services are registered in the `Program.cs` file of the `TaskManager.WebApi` project.

### Database Context

The `KanboardDbContext` is registered as a scoped service. This means that a new instance of the context is created for each HTTP request.

```csharp
builder.Services.AddDbContext<KanboardDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);
```

### Unit of Work

The `IUnitOfWork` interface is registered as a scoped service. It is implemented by the `KanboardDbContext`.

```csharp
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<KanboardDbContext>());
```

### FastEndpoints

FastEndpoints is configured to discover endpoints and validators automatically.

```csharp
builder.Services.AddFastEndpoints(o =>
{
    o.SourceGeneratorDiscoveredTypes = AppDomain
        .CurrentDomain.GetAssemblies()
        .Where(a => a.FullName != null && a.FullName.StartsWith("TaskManager"))
        .SelectMany(a => a.GetTypes())
        .ToList();
});
```

### OpenAPI

OpenAPI (Swagger) is configured for API documentation.

```csharp
builder.Services.AddOpenApi();
```

## Service Lifetimes

The application uses the following service lifetimes:

- **Transient**: A new instance is created each time the service is requested.
- **Scoped**: A new instance is created once per HTTP request.
- **Singleton**: A single instance is created and shared across the entire application.

## Conclusion

The dependency injection configuration in TaskManager ensures that services are properly managed and available throughout the application lifecycle. The use of scoped services for the database context and unit of work ensures that each HTTP request has its own isolated database session.
