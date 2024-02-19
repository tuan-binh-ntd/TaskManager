namespace TaskManager.Core.Events.Issues;

public sealed class IssueDeletedDomainEvent(
    Issue issue,
    Guid userId
    )
    : IDomainEvent
{
    public Issue Issue { get; private set; } = issue;
    public Guid UserId { get; private set; } = userId;
}
