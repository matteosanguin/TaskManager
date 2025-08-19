namespace TaskManager.Application.Commands.Projects.CreateProject;

using FastEndpoints;
using Riok.Mapperly.Abstractions;
using TaskManager.Domain.Entities;
using TaskManager.Shared.Responses;

[Mapper]
public partial class CreateProjectMapper : Mapper<CreateProjectCommand, ProjectResponse, Project>
{
    public partial ProjectResponse ToResponse(Project project);

    public override ProjectResponse FromEntity(Project project)
    {
        return new ProjectResponse(
            project.Id,
            project.Name,
            project.Description,
            project.CreatedAt
        );
    }
}
