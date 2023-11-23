using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class Attachment : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public long Size { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        //Relationship
        public Guid IssueId { get; set; }
        public Issue? Issue { get; set; }
    }
}
