namespace TaskManager.Core.DTOs
{
    public class CreateIssueDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? CompleteDate { get; set; }
        public string Priority { get; set; } = string.Empty;
        public string Watcher { get; set; } = string.Empty;
        public string Voted { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? SprintId { get; set; }
        public Guid IssueTypeId { get; set; }
        public Guid? BacklogId { get; set; }
        public Guid CreatorUserId { get; set; }
    }

    public class UpdateIssueDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? CompleteDate { get; set; }
        public string Priority { get; set; } = string.Empty;
        public string Watcher { get; set; } = string.Empty;
        public string Voted { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? SprintId { get; set; }
        public Guid IssueTypeId { get; set; }
        public Guid? BacklogId { get; set; }
    }
}
