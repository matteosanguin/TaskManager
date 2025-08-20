using System.Reflection; // Aggiungere questo using
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Abstractions;
using TaskManager.Domain.Repositories;
using TaskManager.Infrastructure.Persistence;
using TaskManager.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<KanboardDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<KanboardDbContext>());

// Register repositories
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IColumnRepository, ColumnRepository>();

// Add FastEndpoints
builder.Services.AddFastEndpoints(o =>
{
    o.SourceGeneratorDiscoveredTypes = AppDomain
        .CurrentDomain.GetAssemblies()
        .Where(a => a.FullName != null && a.FullName.StartsWith("TaskManager"))
        .SelectMany(a => a.GetTypes())
        .ToList();
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Configure FastEndpoints
app.UseFastEndpoints(c =>
{
    c.Errors.ResponseBuilder = (failures, ctx, statusCode) =>
    {
        return new { Errors = failures.Select(f => new { f.PropertyName, f.ErrorMessage }) };
    };
});

app.UseExceptionHandler(c =>
    c.Run(async context =>
    {
        var exception = context
            .Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()
            ?.Error;
        if (exception != null)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { Error = exception.Message });
        }
    })
);

TaskManager.Infrastructure.Persistence.DataSeeder.SeedData(app);

app.Run();
