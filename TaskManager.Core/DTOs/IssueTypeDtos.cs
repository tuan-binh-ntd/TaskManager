using Mapster;
using TaskManager.Core.Entities;
using static TaskManager.Core.Extensions.CoreExtensions;

namespace TaskManager.Core.DTOs
{
    public class CreateIssueTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }

    public class UpdateIssueTypeDto : BaseDto<UpdateIssueTypeDto, Issue>
    {
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Icon { get; set; } = string.Empty;

        public override void Register(TypeAdapterConfig config)
        {
            base.Register(config);

            config.NewConfig<UpdateIssueTypeDto, IssueType>()
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Name), dest => dest.Name)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Description), dest => dest.Description!)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Icon), dest => dest.Icon)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreationTime)
                .Ignore(dest => dest.ModificationTime!)
                .IgnoreNullValues(true);
        }
    }
}
