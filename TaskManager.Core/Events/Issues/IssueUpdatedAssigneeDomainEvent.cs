namespace TaskManager.Core.Events.Issues;

public sealed class IssueUpdatedAssigneeDomainEvent(
    Issue issue,
    UpdateIssueDto updateIssueDto
    )
    : IDomainEvent
{
    public Issue Issue { get; private set; } = issue;
    public UpdateIssueDto UpdateIssueDto { get; private set; } = updateIssueDto;
}
