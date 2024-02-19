namespace TaskManager.Application.Priorities.Commands.Update;

public sealed class UpdatePriorityCommand(
    Guid priorityId,
    UpdatePriorityDto updatePriorityDto
    )
    : ICommand<Result<PriorityViewModel>>
{
    public Guid PriorityId { get; private set; } = priorityId;
    public UpdatePriorityDto UpdatePriorityDto { get; private set; } = updatePriorityDto;
}
