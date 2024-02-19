namespace TaskManager.Core.Events.Issues;

public sealed class IssueCreatedDomainEvent(
    Guid creatorUserId,
    Guid? defaultAssigneeId,
    Guid projectId,
    Issue issue
    )
    : IDomainEvent
{
    public Guid CreatorUserId { get; private set; } = creatorUserId;
    public Guid? DefaultAssigneeId { get; private set; } = defaultAssigneeId;
    public Guid ProjectId { get; private set; } = projectId;
    public Issue Issue { get; private set; } = issue;
}
