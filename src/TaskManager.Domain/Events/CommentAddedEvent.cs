using TaskManager.Domain.Entities;
using DomainTask = TaskManager.Domain.Entities.Task;

namespace TaskManager.Domain.Events;

/// <summary>
/// Evento generato quando un commento viene aggiunto a un task
/// </summary>
public class CommentAddedEvent
{
    public Comment Comment { get; }
    public DomainTask Task { get; }
    public User Author { get; }
    public DateTime AddedAt { get; }

    public CommentAddedEvent(Comment comment, DomainTask task, User author)
    {
        Comment = comment;
        Task = task;
        Author = author;
        AddedAt = DateTime.UtcNow;
    }
}
