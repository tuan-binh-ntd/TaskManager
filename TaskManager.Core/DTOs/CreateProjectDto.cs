using System.ComponentModel.DataAnnotations;

namespace TaskManager.Core.DTOs
{
    public class CreateProjectDto
    {
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public string? Code { get; set; }
        public string? AvatarUrl { get; set; }
        public bool IsFavourite { get; set; } = false;
    }
}
