using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        // Navigation properties
        public Project Project { get; set; } = null!;
    }
}
