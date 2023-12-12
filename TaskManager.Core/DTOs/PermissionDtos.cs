namespace TaskManager.Core.DTOs;

public class CreatePermissionDto
{
    public string Name { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
}

public class UpdatePermissionDto
{
    public string Name { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
}
