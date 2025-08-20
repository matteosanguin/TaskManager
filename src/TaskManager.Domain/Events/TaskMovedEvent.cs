using TaskManager.Domain.Entities;
using DomainTask = TaskManager.Domain.Entities.Task;

namespace TaskManager.Domain.Events;

/// <summary>
/// Evento generato quando un task viene spostato da una colonna all'altra
/// </summary>
public class TaskMovedEvent
{
    public DomainTask Task { get; }
    public Guid OriginalColumnId { get; }
    public Guid NewColumnId { get; }
    public DateTime MovedAt { get; }

    public TaskMovedEvent(DomainTask task, Guid originalColumnId, Guid newColumnId)
    {
        Task = task;
        OriginalColumnId = originalColumnId;
        NewColumnId = newColumnId;
        MovedAt = DateTime.UtcNow;
    }
}
