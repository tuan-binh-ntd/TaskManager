namespace TaskManager.Core.DTOs;

public class AddMemberToProjectDto
{
    public Guid ProjectId { get; set; }
    public ICollection<Guid>? UserIds { get; set; }
    public Guid PermissionGroupId { get; set; }
}
