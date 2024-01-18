namespace TaskManager.Core.Entities;

public class Team : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid CreatorUserId { get; set; }
    public ICollection<UserTeam>? UserTeams { get; set; }
}
