namespace TaskManager.Application.Epics.Commands.Update;

internal class UpdateEpicCommandHandler(
    IIssueRepository issueRepository,
    IIssueDetailRepository issueDetailRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<UpdateEpicCommand, Result<EpicViewModel>>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IIssueDetailRepository _issueDetailRepository = issueDetailRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<EpicViewModel>> Handle(UpdateEpicCommand updateEpicCommand, CancellationToken cancellationToken)
    {
        var epic = await _issueRepository.GetByIdAsync(updateEpicCommand.EpicId);
        var issueDetail = await _issueDetailRepository.GetIssueDetailByIssueIdAsync(updateEpicCommand.EpicId);

        if (epic is null) return Result.Failure<EpicViewModel>(Error.NotFound);

        if (issueDetail is null) return Result.Failure<EpicViewModel>(Error.NotFound);

        if (!string.IsNullOrWhiteSpace(updateEpicCommand.UpdateEpicDto.Name))
        {
            epic.EpicUpdatedName(updateEpicCommand.UpdateEpicDto);
        }
        else if (!string.IsNullOrWhiteSpace(updateEpicCommand.UpdateEpicDto.Description))
        {
            epic.EpicUpdatedDescription(updateEpicCommand.UpdateEpicDto);
        }
        else if (updateEpicCommand.UpdateEpicDto.ParentId is Guid)
        {
            epic.EpicUpdatedParent(updateEpicCommand.UpdateEpicDto);
        }
        else if (updateEpicCommand.UpdateEpicDto.StatusId is Guid)
        {
            epic.EpicUpdatedStatus(updateEpicCommand.UpdateEpicDto);
        }
        else if (updateEpicCommand.UpdateEpicDto.PriorityId is Guid)
        {
            epic.EpicUpdatedPriority(updateEpicCommand.UpdateEpicDto);
        }
        else if (
            updateEpicCommand.UpdateEpicDto.StoryPointEstimate is not 0 && issueDetail.StoryPointEstimate is not 0
            || updateEpicCommand.UpdateEpicDto.StoryPointEstimate is not 0 && issueDetail.StoryPointEstimate is 0
            || updateEpicCommand.UpdateEpicDto.StoryPointEstimate is 0 && issueDetail.StoryPointEstimate is not 0
            )
        {
            epic.EpicUpdatedSPE(updateEpicCommand.UpdateEpicDto);
        }
        else if (updateEpicCommand.UpdateEpicDto.ReporterId is Guid)
        {
            epic.EpicUpdatedReporter(updateEpicCommand.UpdateEpicDto);
        }
        else if (updateEpicCommand.UpdateEpicDto.StartDate != DateTime.MinValue)
        {
            epic.EpicUpdatedStartDate(updateEpicCommand.UpdateEpicDto);
        }
        else if (updateEpicCommand.UpdateEpicDto.DueDate != DateTime.MinValue)
        {
            epic.EpicUpdatedDueDate(updateEpicCommand.UpdateEpicDto);
        }
        else if (updateEpicCommand.UpdateEpicDto.AssigneeId != Guid.Empty)
        {
            epic.EpicUpdatedAssignee(updateEpicCommand.UpdateEpicDto);
        }

        _issueRepository.Update(epic);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(epic.Adapt<EpicViewModel>());
    }
}
