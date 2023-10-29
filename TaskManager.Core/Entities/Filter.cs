using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class Filter :BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid CreatorUserId { get; set; }
        public bool Stared { get; set; } = true;
    }
}
