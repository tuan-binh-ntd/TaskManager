using Mapster;
using TaskManager.Core.Entities;
using static TaskManager.Core.Extensions.CoreExtensions;

namespace TaskManager.Core.DTOs
{
    public class UpdateProjectDto : BaseDto<UpdateProjectDto, Project>
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public string? AvatarUrl { get; set; }
        public bool IsFavourite { get; set; }
        public Guid? LeaderId { get; set; }

        public override void Register(TypeAdapterConfig config)
        {
            base.Register(config);

            config.NewConfig<UpdateProjectDto, Project>()
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Name), dest => dest.Name)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Description), dest => dest.Description)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Code), dest => dest.Code)
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.AvatarUrl), dest => dest.AvatarUrl)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreationTime)
                .Ignore(dest => dest.ModificationTime!)
                .IgnoreNullValues(true);
        }
    }
}
