namespace TaskManager.Application.Labels.Commands.Create;

public sealed class CreateLabelCommand(
    Guid projectId,
    CreateLabelDto createLabelDto
    )
    : ICommand<Result<LabelViewModel>>
{
    public Guid ProjectId { get; private set; } = projectId;
    public CreateLabelDto CreateLabelDto { get; private set; } = createLabelDto;
}
