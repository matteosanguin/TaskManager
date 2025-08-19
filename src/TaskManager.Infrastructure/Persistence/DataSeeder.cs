using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence;

public static class DataSeeder
{
    public static void SeedData(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<KanboardDbContext>();

        context.Database.Migrate();

        if (!context.Projects.Any())
        {
            var projects = new List<Project>
            {
                new()
                {
                    Name = "Project Alpha",
                    Description = "This is the first project.",
                    Tasks = new List<TodoTask>
                    {
                        new() { Title = "Task 1 for Alpha", IsDone = true },
                        new() { Title = "Task 2 for Alpha" }
                    }
                },
                new()
                {
                    Name = "Project Beta",
                    Description = "This is the second project.",
                    Tasks = new List<TodoTask>
                    {
                        new() { Title = "Task 1 for Beta" }
                    }
                }
            };
            context.Projects.AddRange(projects);
            context.SaveChanges();
        }
    }
}
