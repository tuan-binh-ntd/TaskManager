namespace TaskManager.Application.Issues.Commands.Update;

internal sealed class UpdateIssueCommandHandler(
    IIssueRepository issueRepository,
    IIssueDetailRepository issueDetailRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<UpdateIssueCommand, Result<IssueViewModel>>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IIssueDetailRepository _issueDetailRepository = issueDetailRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<IssueViewModel>> Handle(UpdateIssueCommand updateIssueCommand, CancellationToken cancellationToken)
    {
        var issue = await _issueRepository.GetByIdAsync(updateIssueCommand.IssueId);
        var issueDetail = await _issueDetailRepository.GetIssueDetailByIssueIdAsync(updateIssueCommand.IssueId);
        if (issue is null) return Result.Failure<IssueViewModel>(Error.NotFound);
        if (issueDetail is null) return Result.Failure<IssueViewModel>(Error.NotFound);

        issue.IssueDetail = issueDetail;

        if (!string.IsNullOrWhiteSpace(updateIssueCommand.UpdateIssueDto.Name))
        {
            issue.IssueUpdatedName(updateIssueCommand.UpdateIssueDto);
        }
        else if (!string.IsNullOrWhiteSpace(updateIssueCommand.UpdateIssueDto.Description))
        {
            issue.IssueUpdatedDescription(updateIssueCommand.UpdateIssueDto);
        }
        else if (updateIssueCommand.UpdateIssueDto.ParentId is Guid)
        {
            issue.IssueUpdatedParent(updateIssueCommand.UpdateIssueDto);
        }
        else if (updateIssueCommand.UpdateIssueDto.StatusId is Guid)
        {
            issue.IssueUpdatedStatus(updateIssueCommand.UpdateIssueDto);
        }
        else if (updateIssueCommand.UpdateIssueDto.IssueTypeId is Guid)
        {
            issue.IssueUpdatedIssueType(updateIssueCommand.UpdateIssueDto);
        }
        else if (updateIssueCommand.UpdateIssueDto.BacklogId is Guid)
        {
            issue.IssueUpdatedBacklog(updateIssueCommand.UpdateIssueDto);
        }
        else if (updateIssueCommand.UpdateIssueDto.PriorityId is Guid)
        {
            issue.IssueUpdatedPriority(updateIssueCommand.UpdateIssueDto);
        }
        else if (updateIssueCommand.UpdateIssueDto.SprintId is Guid)
        {
            issue.IssueUpdatedSprint(updateIssueCommand.UpdateIssueDto);
        }
        else if (
            updateIssueCommand.UpdateIssueDto.StoryPointEstimate is not 0 && issueDetail.StoryPointEstimate is not 0
            || updateIssueCommand.UpdateIssueDto.StoryPointEstimate is not 0 && issueDetail.StoryPointEstimate is 0
            || updateIssueCommand.UpdateIssueDto.StoryPointEstimate is 0 && issueDetail.StoryPointEstimate is not 0
            )
        {
            issue.IssueUpdatedSPE(updateIssueCommand.UpdateIssueDto);
        }
        else if (updateIssueCommand.UpdateIssueDto.ReporterId is Guid)
        {
            issue.IssueUpdatedReporter(updateIssueCommand.UpdateIssueDto);
        }
        else if (updateIssueCommand.UpdateIssueDto.AssigneeId != Guid.Empty)
        {
            issue.IssueUpdatedAssignee(updateIssueCommand.UpdateIssueDto);
        }

        _issueRepository.Update(issue);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(issue.Adapt<IssueViewModel>());
    }
}
