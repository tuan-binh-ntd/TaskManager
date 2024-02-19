namespace TaskManager.Core.DTOs;

public class UpdateProjectDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsFavourite { get; set; }
    public Guid? DefaultAssigneeId { get; set; }
    public Guid? DefaultPriorityId { get; set; }
}
