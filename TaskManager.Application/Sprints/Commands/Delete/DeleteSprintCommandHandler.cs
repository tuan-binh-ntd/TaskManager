namespace TaskManager.Application.Sprints.Commands.Delete;

internal sealed class DeleteSprintCommandHandler(
    ISprintRepository sprintRepository,
    IIssueRepository issueRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<DeleteSprintCommand, Result<Guid>>
{
    private readonly ISprintRepository _sprintRepository = sprintRepository;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(DeleteSprintCommand deleteSprintCommand, CancellationToken cancellationToken)
    {
        var sprint = await _sprintRepository.GetByIdAsync(deleteSprintCommand.SprintId);
        if (sprint is null) return Result.Failure<Guid>(Error.NotFound);
        var issues = await _sprintRepository.GetIssuesBySprintIdAsync(deleteSprintCommand.SprintId);
        _issueRepository.RemoveRange(issues);
        _sprintRepository.Remove(sprint);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(deleteSprintCommand.SprintId);
    }
}
