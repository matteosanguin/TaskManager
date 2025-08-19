namespace TaskManager.WebApi.Endpoints.Projects;

using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Queries.Projects.GetProjects;
using TaskManager.Infrastructure.Persistence;
using TaskManager.Shared.Responses;

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
