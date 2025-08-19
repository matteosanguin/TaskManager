namespace TaskManager.Application.Commands.Projects.CreateProject;

using System.ComponentModel.DataAnnotations;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Shared.Responses;

public record CreateProjectCommand(
    [property: Required] [property: StringLength(100)] string Name,
    [property: StringLength(500)] string? Description
) : ICommand<ProjectResponse>;
