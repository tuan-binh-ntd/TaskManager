namespace TaskManager.Core.DTOs
{
    public class CreatePermissionGroupDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
    }

    public class UpdatePermissionGroupDto
    {
        public string Name { get; set; } = string.Empty;
    }
}
