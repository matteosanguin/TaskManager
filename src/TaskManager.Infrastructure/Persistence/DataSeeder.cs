using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Domain.Entities;
using TaskManager.Domain.ValueObjects;
using DomainTask = TaskManager.Domain.Entities.Task;

namespace TaskManager.Infrastructure.Persistence
{
    public static class DataSeeder
    {
        public static void SeedData(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<KanboardDbContext>();

            context.Database.Migrate();

            if (!context.Projects.Any())
            {
                // Creazione di un utente di esempio
                var user = new User
                {
                    Username = "admin",
                    Email = Email.Create("admin@example.com"),
                    PasswordHash =
                        "AQAAAAEAACcQAAAAEJ1b3vKzQ8G1d8H9h7H2g6F1e5D4c3B2A1zZyYxXwWvVuUtTsSrRqQpPoOnNmMlLkKjJiIhHgGfFeEdDcCbBaA9zZyYxXwWvVuUtTsSrRqQpPoOnNmMlLkKjJiIhHgGfFeEdDcCbBaA9", // Password: Admin123!
                    Role = "Administrator",
                    Theme = "default",
                    TimeZone = "UTC",
                    IsActive = true
                };
                context.Users.Add(user);
                context.SaveChanges();

                // Creazione di progetti di esempio
                var projectAlpha = new Project
                {
                    Name = "Project Alpha",
                    Description = "This is the first project.",
                    Identifier = "PROJ-001",
                    OwnerId = user.Id,
                    IsActive = true,
                    IsPrivate = false
                };

                var projectBeta = new Project
                {
                    Name = "Project Beta",
                    Description = "This is the second project.",
                    Identifier = "PROJ-002",
                    OwnerId = user.Id,
                    IsActive = true,
                    IsPrivate = true
                };

                context.Projects.AddRange(projectAlpha, projectBeta);
                context.SaveChanges();

                // Creazione delle board per i progetti
                var boardAlpha = new Board { Name = "Board Alpha", ProjectId = projectAlpha.Id };

                var boardBeta = new Board { Name = "Board Beta", ProjectId = projectBeta.Id };

                context.Boards.AddRange(boardAlpha, boardBeta);
                context.SaveChanges();

                // Creazione delle colonne per le board
                var columnAlpha1 = new Column
                {
                    Title = "To Do",
                    Position = 1,
                    ProjectId = projectAlpha.Id,
                    Description = "Tasks to be done"
                };

                var columnAlpha2 = new Column
                {
                    Title = "In Progress",
                    Position = 2,
                    ProjectId = projectAlpha.Id,
                    Description = "Tasks in progress"
                };

                var columnAlpha3 = new Column
                {
                    Title = "Done",
                    Position = 3,
                    ProjectId = projectAlpha.Id,
                    Description = "Completed tasks"
                };

                var columnBeta1 = new Column
                {
                    Title = "To Do",
                    Position = 1,
                    ProjectId = projectBeta.Id,
                    Description = "Tasks to be done"
                };

                var columnBeta2 = new Column
                {
                    Title = "In Progress",
                    Position = 2,
                    ProjectId = projectBeta.Id,
                    Description = "Tasks in progress"
                };

                var columnBeta3 = new Column
                {
                    Title = "Done",
                    Position = 3,
                    ProjectId = projectBeta.Id,
                    Description = "Completed tasks"
                };

                context.Columns.AddRange(
                    columnAlpha1,
                    columnAlpha2,
                    columnAlpha3,
                    columnBeta1,
                    columnBeta2,
                    columnBeta3
                );
                context.SaveChanges();

                // Creazione di task di esempio
                var taskAlpha1 = new DomainTask
                {
                    Title = "Task 1 for Alpha",
                    Description = "First task in Project Alpha",
                    ProjectId = projectAlpha.Id,
                    ColumnId = columnAlpha3.Id,
                    AssigneeId = user.Id,
                    CreatorId = user.Id,
                    Position = 1,
                    Priority = Priority.High
                };

                var taskAlpha2 = new DomainTask
                {
                    Title = "Task 2 for Alpha",
                    Description = "Second task in Project Alpha",
                    ProjectId = projectAlpha.Id,
                    ColumnId = columnAlpha1.Id,
                    AssigneeId = user.Id,
                    CreatorId = user.Id,
                    Position = 1,
                    Priority = Priority.Normal
                };

                var taskBeta1 = new DomainTask
                {
                    Title = "Task 1 for Beta",
                    Description = "First task in Project Beta",
                    ProjectId = projectBeta.Id,
                    ColumnId = columnBeta1.Id,
                    CreatorId = user.Id,
                    Position = 1,
                    Priority = Priority.Low
                };

                context.Tasks.AddRange(taskAlpha1, taskAlpha2, taskBeta1);
                context.SaveChanges();

                // Creazione di commenti di esempio
                var comment1 = new Comment
                {
                    TaskId = taskAlpha1.Id,
                    UserId = user.Id,
                    Content = "This task is completed!",
                    Visibility = "Public"
                };

                context.Comments.Add(comment1);
                context.SaveChanges();

                // Creazione di categorie di esempio
                var categoryAlpha1 = new Category
                {
                    Name = "Feature",
                    ProjectId = projectAlpha.Id,
                    Description = "Feature development tasks",
                    Color = "#007bff"
                };

                var categoryAlpha2 = new Category
                {
                    Name = "Bug",
                    ProjectId = projectAlpha.Id,
                    Description = "Bug fixes",
                    Color = "#dc3545"
                };

                context.Categories.AddRange(categoryAlpha1, categoryAlpha2);
                context.SaveChanges();

                // Creazione di tag di esempio
                var tagAlpha1 = new Tag
                {
                    Name = "Urgent",
                    ProjectId = projectAlpha.Id,
                    Color = "#dc3545"
                };

                var tagAlpha2 = new Tag
                {
                    Name = "Important",
                    ProjectId = projectAlpha.Id,
                    Color = "#ffc107"
                };

                context.Tags.AddRange(tagAlpha1, tagAlpha2);
                context.SaveChanges();

                // Creazione di relazioni Task-Tag
                var taskTag1 = new TaskTag { TaskId = taskAlpha1.Id, TagId = tagAlpha1.Id };

                var taskTag2 = new TaskTag { TaskId = taskAlpha2.Id, TagId = tagAlpha2.Id };

                context.TaskTags.AddRange(taskTag1, taskTag2);
                context.SaveChanges();

                // Creazione di swimlane di esempio
                var swimlaneAlpha1 = new Swimlane
                {
                    Name = "Development",
                    ProjectId = projectAlpha.Id,
                    Position = 1,
                    IsActive = true
                };

                var swimlaneAlpha2 = new Swimlane
                {
                    Name = "Testing",
                    ProjectId = projectAlpha.Id,
                    Position = 2,
                    IsActive = true
                };

                context.Swimlanes.AddRange(swimlaneAlpha1, swimlaneAlpha2);
                context.SaveChanges();
            }
        }
    }
}
