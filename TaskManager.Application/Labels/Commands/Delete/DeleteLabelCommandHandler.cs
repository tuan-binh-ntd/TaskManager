namespace TaskManager.Application.Labels.Commands.Delete;

internal sealed class DeleteLabelCommandHandler(
    ILabelRepository labelRepository,
    ILabelIssueRepository labelIssueRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<DeleteLabelCommand, Result<Guid>>
{
    private readonly ILabelRepository _labelRepository = labelRepository;
    private readonly ILabelIssueRepository _labelIssueRepository = labelIssueRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(DeleteLabelCommand deleteLabelCommand, CancellationToken cancellationToken)
    {
        var label = await _labelRepository.GetByIdAsync(deleteLabelCommand.LabelId);
        if (label is null) return Result.Failure<Guid>(Error.NotFound);
        var labelIssues = await _labelIssueRepository.GetLabelIssuesByLabelIdAsync(label.Id);
        _labelIssueRepository.RemoveRange(labelIssues);
        _labelRepository.Remove(label);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(label.Id);
    }
}
