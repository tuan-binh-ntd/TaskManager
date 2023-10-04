namespace TaskManager.Core.DTOs
{
    public class AddMemberToProjectDto
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
