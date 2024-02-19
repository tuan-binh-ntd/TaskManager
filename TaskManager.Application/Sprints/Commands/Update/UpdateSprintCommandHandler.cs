namespace TaskManager.Application.Sprints.Commands.Update;

internal sealed class UpdateSprintCommandHandler(
     ISprintRepository sprintRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<UpdateSprintCommand, Result<SprintViewModel>>
{
    private readonly ISprintRepository _sprintRepository = sprintRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<SprintViewModel>> Handle(UpdateSprintCommand updateSprintCommand, CancellationToken cancellationToken)
    {
        var sprint = await _sprintRepository.GetByIdAsync(updateSprintCommand.SprintId);
        if (sprint is null) return Result.Failure<SprintViewModel>(Error.NotFound);
        sprint = updateSprintCommand.UpdateSprintDto.Adapt(sprint);
        _sprintRepository.Update(sprint);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(sprint.Adapt<SprintViewModel>());
    }
}
