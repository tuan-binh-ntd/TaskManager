using TaskManager.Core.Core;

namespace TaskManager.Core.Entities;

public class Priority : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public bool IsMain { get; set; }

    public Guid? ProjectId { get; set; }
    public Project? Project { get; set; }
}
