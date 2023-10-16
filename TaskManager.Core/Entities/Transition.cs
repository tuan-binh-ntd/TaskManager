using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class Transition : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        //Relationship
        public Guid? ProjectId { get; set; }
        public Project? Project { get; set; }
        public Guid? FromStatusId { get; set; }
        public Status? FromStatus { get; set; }
        public Guid ToStatusId { get; set; }
        public Status? ToStatus { get; set; }
    }
}
