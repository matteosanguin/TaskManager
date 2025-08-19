using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities
{
    public class Board : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }

        // Navigation properties
        public Project Project { get; set; } = null!;
        public ICollection<Column> Columns { get; set; } = new List<Column>();
    }
}
