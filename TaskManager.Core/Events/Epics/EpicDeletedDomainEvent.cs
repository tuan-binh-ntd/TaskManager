namespace TaskManager.Core.Events.Epics;

public sealed class EpicDeletedDomainEvent(
    Issue epic,
    Guid userId
    )
    : IDomainEvent
{
    public Issue Epic { get; private set; } = epic;
    public Guid UserId { get; private set; } = userId;
}
