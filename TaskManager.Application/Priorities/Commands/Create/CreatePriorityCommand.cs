namespace TaskManager.Application.Priorities.Commands.Create;

public sealed class CreatePriorityCommand(
    CreatePriorityDto createPriorityDto
    )
    : ICommand<Result<PriorityViewModel>>
{
    public CreatePriorityDto CreatePriorityDto { get; private set; } = createPriorityDto;
}
