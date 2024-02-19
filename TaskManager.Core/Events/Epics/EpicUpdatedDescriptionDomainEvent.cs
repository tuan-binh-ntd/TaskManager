namespace TaskManager.Core.Events.Epics;

public sealed class EpicUpdatedDescriptionDomainEvent(
    Issue epic,
    UpdateEpicDto updateEpicDto
    )
    : IDomainEvent
{
    public Issue Epic { get; private set; } = epic;
    public UpdateEpicDto UpdateEpicDto { get; private set; } = updateEpicDto;
}
