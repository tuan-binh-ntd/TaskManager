using System.ComponentModel.DataAnnotations;

namespace TaskManager.Core.DTOs
{
    public class AppRoleDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
