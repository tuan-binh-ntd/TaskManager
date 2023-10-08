namespace TaskManager.Core.DTOs
{
    public class AddMemberToProjectDto
    {
        public Guid ProjectId { get; set; }
        public string Role { get; set; } = string.Empty;
        public ICollection<MemberDto>? Members { get; set; }
    }

    public class MemberDto
    {
        public Guid UserId { get; set; }
    }
}
