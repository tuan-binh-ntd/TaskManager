using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class Comment : BaseEntity
    {
        public Guid CreatorUserId { get; set; }
        public bool IsEdited { get; set; }
        public string Content { get; set; } = string.Empty;

        //Relationship
        public Guid IssueId { get; set; }
        public Issue? Issue { get; set; }
    }
}
