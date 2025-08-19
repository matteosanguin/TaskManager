namespace TaskManager.WebApi.Endpoints.Projects;

using FastEndpoints;
using TaskManager.Shared.Responses;

public class GetProjectByIdEndpoint : Endpoint<GetProjectByIdRequest, ProjectResponse>
{
    public override void Configure()
    {
        Get("/projects/{id}");
        AllowAnonymous(); // Temporaneo
        Summary(s =>
        {
            s.Summary = "Ottieni un progetto per ID";
            s.Description = "Ottieni i dettagli di un progetto TaskManager tramite il suo ID.";
        });
        // Non implementato per ora
    }

    public override async Task HandleAsync(GetProjectByIdRequest req, CancellationToken ct)
    {
        HttpContext.Response.StatusCode = 404; // Placeholder
        await HttpContext.Response.CompleteAsync();
    }
}

public record GetProjectByIdRequest(Guid Id);
