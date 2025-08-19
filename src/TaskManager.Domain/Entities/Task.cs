using TaskManager.Domain.Common;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Domain.Entities
{
    public class Task : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public Guid ColumnId { get; set; }
        public Guid? AssigneeId { get; set; }
        public Guid CreatorId { get; set; }
        public int Position { get; set; }
        public Priority Priority { get; set; } = Priority.Normal;
        public DateTime? DueDate { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
        public int? TimeEstimated { get; set; }
        public int? TimeSpent { get; set; }

        // Navigation properties
        public Project Project { get; set; } = null!;
        public Column Column { get; set; } = null!;
        public User? Assignee { get; set; }
        public User Creator { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}
