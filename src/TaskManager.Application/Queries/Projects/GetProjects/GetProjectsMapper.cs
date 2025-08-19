namespace TaskManager.Application.Queries.Projects.GetProjects;

using FastEndpoints;
using Riok.Mapperly.Abstractions;
using TaskManager.Domain.Entities;
using TaskManager.Shared.Responses;

[Mapper]
public partial class GetProjectsMapper
    : Mapper<GetProjectsQuery, IEnumerable<ProjectResponse>, IEnumerable<Project>>
{
    public partial IEnumerable<ProjectResponse> ToResponse(IEnumerable<Project> projects);

    public new IEnumerable<ProjectResponse> FromEntity(IEnumerable<Project> projects)
    {
        return ToResponse(projects);
    }
}
