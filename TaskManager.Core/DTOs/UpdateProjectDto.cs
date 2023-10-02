using System.ComponentModel.DataAnnotations;

namespace TaskManager.Core.DTOs
{
    public class UpdateProjectDto
    {
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public string? Code { get; set; }
        public string? AvatarUrl { get; set; }
        [Required]
        public bool IsFavourite { get; set; }
    }
}
