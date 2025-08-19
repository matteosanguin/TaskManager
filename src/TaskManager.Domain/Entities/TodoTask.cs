using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities;

public class TodoTask : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public bool IsDone { get; set; }
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
}
