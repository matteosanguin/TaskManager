using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities
{
    public class Swimlane : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public int Position { get; set; }
        public bool IsActive { get; set; } = true;
        public int? TaskLimit { get; set; }

        // Navigation properties
        public Project Project { get; set; } = null!;
    }
}
