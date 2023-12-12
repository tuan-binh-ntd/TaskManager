using System.ComponentModel.DataAnnotations;

namespace TaskManager.Core.DTOs;

public class CreateAppRoleDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
