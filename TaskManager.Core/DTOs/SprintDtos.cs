using Mapster;
using TaskManager.Core.Entities;
using static TaskManager.Core.Extensions.CoreExtensions;

namespace TaskManager.Core.DTOs;

public class CreateSprintDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Goal { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
}

public class UpdateSprintDto : BaseDto<UpdateSprintDto, Sprint>
{
    public string? Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Goal { get; set; }
    public Guid? ProjectId { get; set; }

    public override void Register(TypeAdapterConfig config)
    {
        base.Register(config);

        config.NewConfig<UpdateSprintDto, Sprint>()
            .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Name), dest => dest.Name)
            .IgnoreIf((src, dest) => src.StartDate == null, dest => dest.StartDate!)
            .IgnoreIf((src, dest) => src.EndDate == null, dest => dest.EndDate!)
            .IgnoreIf((src, dest) => string.IsNullOrWhiteSpace(src.Goal), dest => dest.Goal!)
            .IgnoreIf((src, dest) => src.ProjectId == null, dest => dest.ProjectId!)
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.CreationTime)
            .Ignore(dest => dest.ModificationTime!)
            .IgnoreNullValues(true);
    }
}

public class CompleteSprintDto
{
    /// <summary>
    /// Specific sprint name, New sprint or Backlog string
    /// </summary>
    public string? Option { get; set; } = string.Empty;
    /// <summary>
    /// Field is not null when option is specific sprint name
    /// </summary>
    public Guid? SprintId { get; set; }
}
