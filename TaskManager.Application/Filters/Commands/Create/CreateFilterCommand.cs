namespace TaskManager.Application.Filters.Commands.Create;

public sealed class CreateFilterCommand(
    CreateFilterDto createFilterDto
    )
    : ICommand<Result<FilterViewModel>>
{
    public CreateFilterDto CreateFilterDto { get; private set; } = createFilterDto;
}
