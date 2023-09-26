using System.ComponentModel.DataAnnotations;

namespace TaskManager.Infrastructure.DTOs
{
    public class PasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
