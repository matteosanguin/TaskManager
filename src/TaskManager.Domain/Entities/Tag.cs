using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string Color { get; set; } = string.Empty;

        // Navigation properties
        public Project Project { get; set; } = null!;
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
