namespace TaskManager.Application.Issues.Commands.Delete;

internal sealed class DeleteIssueCommandHandler(
    IIssueRepository issueRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<DeleteIssueCommand, Result<Guid>>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(DeleteIssueCommand deleteIssueCommand, CancellationToken cancellationToken)
    {
        var issue = await _issueRepository.GetByIdAsync(deleteIssueCommand.IssueId);
        if (issue is null) return Result.Failure<Guid>(Error.NotFound);

        await _issueRepository
            .DeleteChildIssueAsync(issue.Id);
        _issueRepository.Remove(issue);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(issue.Id);
    }
}
