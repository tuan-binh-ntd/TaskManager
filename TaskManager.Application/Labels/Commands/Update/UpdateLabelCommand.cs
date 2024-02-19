namespace TaskManager.Application.Labels.Commands.Update;

public class UpdateLabelCommand(
    Guid labelId,
    UpdateLabelDto updateLabelDto
    )
    : ICommand<Result<LabelViewModel>>
{
    public Guid LabelId { get; private set; } = labelId;
    public UpdateLabelDto UpdateLabelDto { get; private set; } = updateLabelDto;
}
