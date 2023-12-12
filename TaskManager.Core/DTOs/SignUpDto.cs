using System.ComponentModel.DataAnnotations;

namespace TaskManager.Core.DTOs;

public class SignUpDto
{
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
}
