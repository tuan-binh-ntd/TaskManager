using Mapster;
using TaskManager.Core.Entities;
using static TaskManager.Core.Extensions.CoreExtensions;

namespace TaskManager.Core.DTOs
{
    public class CreateStatusDto
    {
        public string Name { get; set; } = string.Empty;
        //Relationship
        public Guid? ProjectId { get; set; }
        public Guid StatusCategoryId { get; set; }
    }

    public class UpdateStatusDto : BaseDto<UpdateStatusDto, Status>
    {
        public string Name { get; set; } = string.Empty;
        //Relationship
        public Guid? StatusCategoryId { get; set; }

        public override void Register(TypeAdapterConfig config)
        {
            base.Register(config);

            config.NewConfig<UpdateStatusDto, Status>()
                .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Name), dest => dest.Name)
                .IgnoreIf((src, dest) => src.StatusCategoryId == null, dest => dest.StatusCategoryId!)
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.CreationTime)
                .Ignore(dest => dest.ModificationTime!)
                .IgnoreNullValues(true);
        }
    }
}
