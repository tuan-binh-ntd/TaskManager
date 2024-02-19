namespace TaskManager.Core.Core;

public interface IHasDomainEvent
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();

    void AddDomainEvent(IDomainEvent domainEvent);
}
