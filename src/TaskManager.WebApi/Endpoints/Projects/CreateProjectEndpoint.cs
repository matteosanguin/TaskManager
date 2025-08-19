namespace TaskManager.WebApi.Endpoints.Projects;

using FastEndpoints;
using TaskManager.Application.Commands.Projects.CreateProject;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence;
using TaskManager.Shared.Responses;

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
            s.ExampleRequest = new CreateProjectCommand(
                "My New Project",
                "A description for my new project."
            );
            s.ResponseExamples[201] = new ProjectResponse(
                Guid.NewGuid(),
                "My New Project",
                "A description for my new project.",
                DateTime.UtcNow
            );
        });
    }

    public override async Task<ProjectResponse> ExecuteAsync(
        CreateProjectCommand req,
        CancellationToken ct
    )
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
        return response;
    }
}
