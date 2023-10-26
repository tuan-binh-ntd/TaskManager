using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class Priority : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public Guid ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}
