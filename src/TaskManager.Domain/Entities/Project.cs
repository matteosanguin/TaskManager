using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Identifier { get; set; } = string.Empty;
        public Guid OwnerId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsPrivate { get; set; } = false;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TaskLimit { get; set; }
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User Owner { get; set; } = null!;
        public Board Board { get; set; } = null!;
        public ICollection<Column> Columns { get; set; } = new List<Column>();
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<Swimlane> Swimlanes { get; set; } = new List<Swimlane>();
    }
}
