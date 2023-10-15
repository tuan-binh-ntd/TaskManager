using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class Sprint : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Goal { get; set; } = string.Empty;

        // Relationship
        public Project? Project { get; set; }
        public Guid ProjectId { get; set; }
        public ICollection<Issue>? Issues { get; set; }
    }
}
