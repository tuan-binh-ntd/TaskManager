namespace TaskManager.Core.Events.Issues;

public sealed class IssueUpdatedParentDomainEvent(
    Issue issue,
    UpdateIssueDto updateIssueDto
    )
    : IDomainEvent
{
    public Issue Issue { get; private set; } = issue;
    public UpdateIssueDto UpdateIssueDto { get; private set; } = updateIssueDto;
}
