namespace TaskManager.Application.IssueTypes.Commands.Update;

public class UpdateIssueTypeCommand(
    Guid issueTypeId,
    UpdateIssueTypeDto updateIssueTypeDto
    )
    : ICommand<Result<IssueTypeViewModel>>
{
    public Guid IssueTypeId { get; private set; } = issueTypeId;
    public UpdateIssueTypeDto UpdateIssueTypeDto { get; private set; } = updateIssueTypeDto;
}
