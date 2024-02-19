namespace TaskManager.Application.Labels.Commands.Delete;

public sealed class DeleteLabelCommand(
    Guid labelId
    )
    : ICommand<Result<Guid>>
{
    public Guid LabelId { get; private set; } = labelId;
}
