using System.ComponentModel.DataAnnotations;

namespace TaskManager.Core.DTOs
{
    public class PasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
