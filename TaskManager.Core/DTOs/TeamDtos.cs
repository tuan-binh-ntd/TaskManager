namespace TaskManager.Core.DTOs
{
    public class CreateTeamDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid CreatorUserId { get; set; }
        public IEnumerable<Guid>? UserIds { get; set; }
    }

    public class UpdateTeamDto
    {
        public string Name { get; set; } = string.Empty;
        public IEnumerable<Guid>? UserIds { get; set; }
    }

    public class AddMemberToTeamDto
    {
        public Guid TeamId { get; set; }
        public IEnumerable<Guid> UserIds { get; set; } = new List<Guid>();
    }
}
