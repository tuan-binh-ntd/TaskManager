namespace TaskManager.Core.Events.Statuses;

public sealed class StatusDeletedDomainEvent(
    Guid statusId,
    Guid newStatusId
    )
    : IDomainEvent
{
    public Guid StatusId { get; private set; } = statusId;
    public Guid NewStatusId { get; private set; } = newStatusId;
}
