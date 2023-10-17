﻿using TaskManager.Core.Core;

namespace TaskManager.Core.Entities
{
    public class Issue : BaseEntity
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        //public override Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime? CompleteDate { get; set; }
        public string? Priority { get; set; } = string.Empty;
        public string? Watcher { get; set; } = string.Empty;
        public string? Voted { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        //Relationship
        public Guid? ParentId { get; set; }
        public Guid? SprintId { get; set; }
        public Sprint? Sprint { get; set; }
        public Guid IssueTypeId { get; set; }
        public IssueType? IssueType { get; set; }
        public Guid? BacklogId { get; set; }
        public Backlog? Backlog { get; set; }
        public ICollection<IssueHistory>? IssueHistories { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Attachment>? Attachments { get; set; }
        public IssueDetail? IssueDetail { get; set; }
        public Guid? StatusId { get; set; }
        public Status? Status { get; set; }
    }
}
