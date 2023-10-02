namespace TaskManager.Core.DTOs
{
    public class UpdateProjectDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public string? AvatarUrl { get; set; }
        public bool? IsFavourite { get; set; }
    }
}
