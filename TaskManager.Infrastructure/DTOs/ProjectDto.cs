using System.ComponentModel.DataAnnotations;

namespace TaskManager.Infrastructure.DTOs
{
    public class ProjectDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Code { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
    }
}
