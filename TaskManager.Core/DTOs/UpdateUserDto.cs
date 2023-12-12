namespace TaskManager.Core.DTOs;

public class UpdateUserDto
{
    public string? Name { get; set; } = string.Empty;
    public string? Department { get; set; } = string.Empty;
    public string? Organization { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; } = string.Empty;
    public string? JobTitle { get; set; } = string.Empty;
    public string? Location { get; set; } = string.Empty;
}
