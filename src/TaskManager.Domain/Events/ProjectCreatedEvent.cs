using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Events;

/// <summary>
/// Evento generato quando un progetto viene creato
/// </summary>
public class ProjectCreatedEvent
{
    public Project Project { get; }
    public User Creator { get; }
    public DateTime CreatedAt { get; }

    public ProjectCreatedEvent(Project project, User creator)
    {
        Project = project;
        Creator = creator;
        CreatedAt = DateTime.UtcNow;
    }
}
