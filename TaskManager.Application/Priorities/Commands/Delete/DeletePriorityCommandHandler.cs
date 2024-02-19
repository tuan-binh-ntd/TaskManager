namespace TaskManager.Application.Priorities.Commands.Delete;

internal sealed class DeletePriorityCommandHandler(
    IPriorityRepository priorityRepository,
    IIssueRepository issueRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<DeletePriorityCommand, Result<Guid>>
{
    private readonly IPriorityRepository _priorityRepository = priorityRepository;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(DeletePriorityCommand deletePriorityCommand, CancellationToken cancellationToken)
    {
        var priority = await _priorityRepository.GetByIdAsync(deletePriorityCommand.PriorityId);

        if (priority is null) return Result.Failure<Guid>(Error.NotFound);

        int count = await _issueRepository.CountIssueByPriorityIdAsync(deletePriorityCommand.PriorityId);
        if (count > 0)
        {
            await _issueRepository.UpdateOneColumnForIssueAsync(deletePriorityCommand.PriorityId, deletePriorityCommand.NewPriorityId, NameColumn.PriorityId);
        }
        _priorityRepository.Remove(priority);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(priority.Id);
    }
}
