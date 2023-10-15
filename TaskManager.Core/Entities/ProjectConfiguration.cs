using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class ProjectConfiguration : BaseEntity
    {
        public Guid ProjectId { get; set; }
        public int IssueCode { get; set; }
        public int SprintCode { get; set; }
        public string Code { get; set; } = string.Empty;
        //Relationship
        public Project? Project { get; set; }
    }
}
