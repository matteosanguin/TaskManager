namespace TaskManager.WebApi.Endpoints.Projects;

using FastEndpoints;
using TaskManager.Application.Commands.Projects.CreateProject;
using TaskManager.Domain.Entities;
using TaskManager.Shared.Responses;

public class CreateProjectMapper : Mapper<CreateProjectCommand, ProjectResponse, Project>
{
    public override ProjectResponse FromEntity(Project project)
    {
        ArgumentNullException.ThrowIfNull(project);

        return new ProjectResponse(
            project.Id,
            project.Name,
            project.Description ?? string.Empty,
            project.CreatedAt
        );
    }
}
