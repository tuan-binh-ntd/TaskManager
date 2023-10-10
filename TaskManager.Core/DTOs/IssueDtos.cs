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
        public string? Watcher { get; set; } = string.Empty;
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
        public string? Priority { get; set; }
        public string? Watcher { get; set; }
        public string? Voted { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? SprintId { get; set; }
        public Guid? IssueTypeId { get; set; }
        public Guid? BacklogId { get; set; }

        public override void Register(TypeAdapterConfig config)
        {
            base.Register(config);

            config.NewConfig<UpdateIssueDto, Issue>()
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Name), dest => dest.Name)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Description), dest => dest.Description!)
                .IgnoreIf((src, dest) => src.CompleteDate == null, dest => dest.CompleteDate!)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Priority), dest => dest.Priority!)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Watcher), dest => dest.Watcher!)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Voted), dest => dest.Voted!)
                .IgnoreIf((src, dest) => src.StartDate == null, dest => dest.StartDate!)
                .IgnoreIf((src, dest) => src.DueDate == null, dest => dest.DueDate!)
                .IgnoreIf((src, dest) => src.ParentId == null, dest => dest.ParentId!)
                .IgnoreIf((src, dest) => src.SprintId == null, dest => dest.SprintId!)
                .IgnoreIf((src, dest) => src.IssueTypeId == null, dest => dest.IssueTypeId!)
                .IgnoreIf((src, dest) => src.BacklogId == null, dest => dest.BacklogId!)
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
    }
}
