namespace TaskManager.Application.Issues.Commands.Update;

public sealed class UpdateIssueCommand(
    Guid issueId,
    UpdateIssueDto updateIssueDto
    )
    : ICommand<Result<IssueViewModel>>
{
    public Guid IssueId { get; private set; } = issueId;
    public UpdateIssueDto UpdateIssueDto { get; private set; } = updateIssueDto;
}
