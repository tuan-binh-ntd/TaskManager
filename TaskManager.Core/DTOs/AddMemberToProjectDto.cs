namespace TaskManager.Core.DTOs
{
    public class AddMemberToProjectDto
    {
        public Guid ProjectId { get; set; }
        public ICollection<MemberDto>? Members { get; set; }
    }

    public class MemberDto
    {
        public string Role { get; set; } = string.Empty;
        public Guid UserId { get; set; }
    }
}
