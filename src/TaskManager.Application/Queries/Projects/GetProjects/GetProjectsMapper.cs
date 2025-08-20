namespace TaskManager.Application.Queries.Projects.GetProjects;

using FastEndpoints;
using Riok.Mapperly.Abstractions;
using TaskManager.Domain.Entities;
using TaskManager.Shared.Responses;

[Mapper]
public partial class GetProjectsMapper
    : Mapper<GetProjectsQuery, IEnumerable<ProjectResponse>, IEnumerable<Project>>
{
    [MapperIgnoreSource(nameof(Project.Identifier))]
    [MapperIgnoreSource(nameof(Project.OwnerId))]
    [MapperIgnoreSource(nameof(Project.IsActive))]
    [MapperIgnoreSource(nameof(Project.IsPrivate))]
    [MapperIgnoreSource(nameof(Project.StartDate))]
    [MapperIgnoreSource(nameof(Project.EndDate))]
    [MapperIgnoreSource(nameof(Project.TaskLimit))]
    [MapperIgnoreSource(nameof(Project.ModifiedAt))]
    [MapperIgnoreSource(nameof(Project.UpdatedAt))]
    [MapperIgnoreSource(nameof(Project.Owner))]
    [MapperIgnoreSource(nameof(Project.Board))]
    [MapperIgnoreSource(nameof(Project.Columns))]
    [MapperIgnoreSource(nameof(Project.Tasks))]
    [MapperIgnoreSource(nameof(Project.Categories))]
    [MapperIgnoreSource(nameof(Project.Tags))]
    [MapperIgnoreSource(nameof(Project.Swimlanes))]
    public partial ProjectResponse ToResponse(Project project);

    public partial IEnumerable<ProjectResponse> ToResponse(IEnumerable<Project> projects);

    public new IEnumerable<ProjectResponse> FromEntity(IEnumerable<Project> projects)
    {
        return ToResponse(projects);
    }
}
