namespace TaskManager.Application.IssueTypes.Commands.Delete;

public sealed class DeleteIssueTypeCommand(
    Guid issueTypeId,
    Guid? newIssueTypeId
    )
    : ICommand<Result<Guid>>
{
    public Guid IssueTypeId { get; private set; } = issueTypeId;
    public Guid? NewIssueTypeId { get; private set; } = newIssueTypeId;
}
