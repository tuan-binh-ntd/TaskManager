using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class Status : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        //Relationship
        public Guid? ProjectId { get; set; }
        public Project? Project { get; set; }
        public Guid StatusCategoryId { get; set; }
        public StatusCategory? StatusCategory { get; set; }
        public ICollection<Issue>? Issues { get; set; }
    }
}
