using TaskManager.Domain.Entities;
using DomainTask = TaskManager.Domain.Entities.Task;

namespace TaskManager.Domain.Events;

/// <summary>
/// Evento generato quando un task viene assegnato a un utente
/// </summary>
public class TaskAssignedEvent
{
    public DomainTask Task { get; }
    public User Assignee { get; }
    public DateTime AssignedAt { get; }

    public TaskAssignedEvent(DomainTask task, User assignee)
    {
        Task = task;
        Assignee = assignee;
        AssignedAt = DateTime.UtcNow;
    }
}
