namespace TaskManager.Core.DTOs;

public class CreateIssueTypeDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}

public class UpdateIssueTypeDto
{
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string? Icon { get; set; } = string.Empty;
}
