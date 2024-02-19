namespace TaskManager.Application.IssueTypes.Commands.Delete;

internal sealed class DeleteIssueTypeCommandHandler(
    IIssueRepository issueRepository,
    IIssueTypeRepository issueTypeRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<DeleteIssueTypeCommand, Result<Guid>>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IIssueTypeRepository _issueTypeRepository = issueTypeRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(DeleteIssueTypeCommand deleteIssueTypeCommand, CancellationToken cancellationToken)
    {
        var issueType = await _issueTypeRepository.GetByIdAsync(deleteIssueTypeCommand.IssueTypeId);
        if (issueType is null) return Result.Failure<Guid>(Error.NotFound);
        int count = await _issueRepository.CountIssueByIssueTypeIdAsync(deleteIssueTypeCommand.IssueTypeId);
        if (count > 0 && deleteIssueTypeCommand.NewIssueTypeId is Guid newId)
        {
            await _issueRepository.UpdateOneColumnForIssueAsync(deleteIssueTypeCommand.IssueTypeId, newId, NameColumn.IssueTypeId);
        }
        _issueTypeRepository.Remove(issueType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(deleteIssueTypeCommand.IssueTypeId);
    }
}
