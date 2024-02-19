namespace TaskManager.Core.Events.Users;

public sealed class UserRegisteredDomainEvent(
    Guid userId
    )
     : IDomainEvent
{
    public Guid UserId { get; private set; } = userId;
}
