namespace TaskManager.Core.ViewModel
{
    public class IssueViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string Priority { get; set; } = string.Empty;
        public string Watcher { get; set; } = string.Empty;
        public string Voted { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }

        //Relationship
        //public Guid? ParentId { get; set; }
        //public Guid? SprintId { get; set; }
        //public Sprint? Sprint { get; set; }
        //public Guid IssueTypeId { get; set; }
        //public IssueType? IssueType { get; set; }
        //public Guid? BacklogId { get; set; }
        //public Backlog? Backlog { get; set; }
        //public ICollection<IssueHistory>? IssueHistories { get; set; }
        //public ICollection<Comment>? Comments { get; set; }
        //public ICollection<Attachment>? Attachments { get; set; }
        //public IssueDetail? IssueDetail { get; set; }
    }

    public class IssueHistoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public Guid CreatorUserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; }
    }

    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public Guid CreatorUserId { get; set; }
        public bool IsEdited { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    public class AttachmentViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
    }

    public class IssueDetailViewModel
    {
        public Guid Id { get; set; }
        public Guid? AssigneeId { get; set; }
        public Guid ReporterId { get; set; }
        public int StoryPointEstimate { get; set; }
        public string Label { get; set; } = string.Empty;
    }
}
