namespace TaskManager.Application.Issues.Commands.Delete;

public sealed class DeleteIssueCommand(
    Guid issueId,
    Guid userId
    )
    : ICommand<Result<Guid>>
{
    public Guid IssueId { get; private set; } = issueId;
    public Guid UserId { get; private set; } = userId;
}
