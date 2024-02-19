namespace TaskManager.Application.Sprints.Commands.Start;

internal sealed class StartSprintCommandHandler(
    ISprintRepository sprintRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<StartSprintCommand, Result<SprintViewModel>>
{
    private readonly ISprintRepository _sprintRepository = sprintRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<SprintViewModel>> Handle(StartSprintCommand startSprintCommand, CancellationToken cancellationToken)
    {
        var sprint = await _sprintRepository.GetByIdAsync(startSprintCommand.SprintId);
        if (sprint is null) return Result.Failure<SprintViewModel>(Error.NotFound);
        sprint = startSprintCommand.UpdateSprintDto.Adapt(sprint);
        sprint.Start();
        _sprintRepository.Update(sprint);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(sprint.Adapt<SprintViewModel>());
    }
}
