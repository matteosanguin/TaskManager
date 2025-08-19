using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Visibility { get; set; } = "Public";

        // Navigation properties
        public Task Task { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
