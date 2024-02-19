namespace TaskManager.Application.Epics.Commands.Update;

public class UpdateEpicCommand(
    Guid epicId,
    UpdateEpicDto updateEpicDto
    )
    : ICommand<Result<EpicViewModel>>
{
    public Guid EpicId { get; private set; } = epicId;
    public UpdateEpicDto UpdateEpicDto { get; private set; } = updateEpicDto;
}
