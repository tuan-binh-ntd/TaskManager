namespace TaskManager.Core.DTOs;

public class CreatePermissionGroupDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public PermissionGroupDto? Timeline { get; set; }
    [Required]
    public PermissionGroupDto? Backlog { get; set; }
    [Required]
    public PermissionGroupDto? Board { get; set; }
    [Required]
    public PermissionGroupDto? Project { get; set; }
    public IReadOnlyCollection<Guid> UserIds { get; set; } = new List<Guid>();
}

public class UpdatePermissionGroupDto
{
    public string? Name { get; set; }
    [Required]
    public PermissionGroupDto? Timeline { get; set; }
    [Required]
    public PermissionGroupDto? Backlog { get; set; }
    [Required]
    public PermissionGroupDto? Board { get; set; }
    [Required]
    public PermissionGroupDto? Project { get; set; }
    public IReadOnlyCollection<Guid> UserIds { get; set; } = new List<Guid>();
}

public class PermissionGroupDto
{
    public bool ViewPermission { get; set; }
    public bool EditPermission { get; set; }
}
