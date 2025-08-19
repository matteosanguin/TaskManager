using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities
{
    public class Column : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public int Position { get; set; }
        public Guid ProjectId { get; set; }
        public int? TaskLimit { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool HideInDashboard { get; set; } = false;

        // Navigation properties
        public Project Project { get; set; } = null!;
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
