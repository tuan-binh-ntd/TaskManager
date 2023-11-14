using Mapster;
using TaskManager.Core.Entities;
using static TaskManager.Core.Extensions.CoreExtensions;

namespace TaskManager.Core.DTOs
{
    public class CreateIssueDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime? CompleteDate { get; set; }
        public string? Priority { get; set; } = string.Empty;
        public string? Voted { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? ParentId { get; set; }
        public Guid IssueTypeId { get; set; }
        public Guid CreatorUserId { get; set; }
    }

    public class UpdateIssueDto : BaseDto<UpdateIssueDto, Issue>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CompleteDate { get; set; }
        public ICollection<Guid>? UserIds { get; set; }
        public string? Voted { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? SprintId { get; set; }
        public Guid? IssueTypeId { get; set; }
        public Guid? BacklogId { get; set; }
        public Guid? AssigneeId { get; set; }
        public Guid? StatusId { get; set; }
        public Guid? PriorityId { get; set; }
        public int? StoryPointEstimate { get; set; }
        public Guid? VersionId { get; set; }
        public Guid? ReporterId { get; set; }

        public override void Register(TypeAdapterConfig config)
        {
            base.Register(config);

            config.NewConfig<UpdateIssueDto, Issue>()
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Name), dest => dest.Name)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Description), dest => dest.Description!)
                .IgnoreIf((src, dest) => src.CompleteDate == null, dest => dest.CompleteDate!)
                .IgnoreIf((src, dest) => src.PriorityId == null, dest => dest.PriorityId!)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Voted), dest => dest.Voted!)
                .IgnoreIf((src, dest) => src.StartDate == null, dest => dest.StartDate!)
                .IgnoreIf((src, dest) => src.DueDate == null, dest => dest.DueDate!)
                .IgnoreIf((src, dest) => src.ParentId == null, dest => dest.ParentId!)
                .IgnoreIf((src, dest) => src.SprintId == null, dest => dest.SprintId!)
                .IgnoreIf((src, dest) => src.IssueTypeId == null, dest => dest.IssueTypeId!)
                .IgnoreIf((src, dest) => src.BacklogId == null, dest => dest.BacklogId!)
                .IgnoreIf((src, dest) => src.VersionId == null, dest => dest.VersionId!)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreationTime)
                .Ignore(dest => dest.ModificationTime!)
                .IgnoreNullValues(true);
        }
    }

    public class CreateIssueByNameDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid IssueTypeId { get; set; }
        public Guid CreatorUserId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? ParentId { get; set; }
    }

    public class CreateChildIssueDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid CreatorUserId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ParentId { get; set; }
    }

    public class CreateEpicDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid CreatorUserId { get; set; }
        public Guid ProjectId { get; set; }
    }

    public class UpdateEpicDto : BaseDto<UpdateEpicDto, Issue>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CompleteDate { get; set; }
        public ICollection<Guid>? UserIds { get; set; }
        public string? Voted { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? AssigneeId { get; set; }
        public Guid? StatusId { get; set; }
        public Guid? PriorityId { get; set; }
        public int? StoryPointEstimate { get; set; }
        public Guid? VersionId { get; set; }
        public Guid? ReporterId { get; set; }

        public override void Register(TypeAdapterConfig config)
        {
            base.Register(config);

            config.NewConfig<UpdateEpicDto, Issue>()
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Name), dest => dest.Name)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Description), dest => dest.Description!)
                .IgnoreIf((src, dest) => src.CompleteDate == null, dest => dest.CompleteDate!)
                .IgnoreIf((src, dest) => src.PriorityId == null, dest => dest.PriorityId!)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Voted), dest => dest.Voted!)
                .IgnoreIf((src, dest) => src.StartDate == null, dest => dest.StartDate!)
                .IgnoreIf((src, dest) => src.DueDate == null, dest => dest.DueDate!)
                .IgnoreIf((src, dest) => src.ParentId == null, dest => dest.ParentId!)
                .IgnoreIf((src, dest) => src.VersionId == null, dest => dest.VersionId!)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreationTime)
                .Ignore(dest => dest.ModificationTime!)
                .IgnoreNullValues(true);
        }
    }

    public class AddIssueToEpicDto
    {
        public Guid IssueId { get; set; }
    }
}
