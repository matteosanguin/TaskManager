namespace TaskManager.Application.Queries.Projects.GetProjects;

using TaskManager.Application.Common.Interfaces;
using TaskManager.Shared.Responses;

public record GetProjectsQuery(bool Dummy = false) : IQuery<IEnumerable<ProjectResponse>>;
