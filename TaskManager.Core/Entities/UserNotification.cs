namespace TaskManager.Core.Entities;

public class UserNotification : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid CreatorUserId { get; set; }
    public Guid IssueId { get; set; }
    public bool IsRead { get; set; }
    // Relationship
    public Guid UserId { get; set; }
    public AppUser? User { get; set; }
}
