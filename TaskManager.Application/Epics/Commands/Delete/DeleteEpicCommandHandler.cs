namespace TaskManager.Application.Epics.Commands.Delete;

internal sealed class DeleteEpicCommandHandler(
    IIssueRepository issueRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<DeleteEpicCommand, Result<Guid>>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(DeleteEpicCommand deleteEpicCommand, CancellationToken cancellationToken)
    {
        var epic = await _issueRepository.GetByIdAsync(deleteEpicCommand.EpicId);

        if (epic is null) return Result.Failure<Guid>(Error.NotFound);

        await _issueRepository.UpdateOneColumnForIssueAsync(deleteEpicCommand.EpicId, null, NameColumn.ParentId);
        epic.EpicDeleted(deleteEpicCommand.UserId);
        _issueRepository.Remove(epic);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(deleteEpicCommand.EpicId);
    }
}
