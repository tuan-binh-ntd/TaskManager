namespace TaskManager.Application.Issues.Commands.Create;

public sealed class CreateIssueCommand(
    Guid? sprintId,
    Guid? backlogId,
    CreateIssueByNameDto createIssueByNameDto
    )
    : ICommand<Result<IssueViewModel>>
{
    public Guid? SprintId { get; private set; } = sprintId;
    public Guid? BacklogId { get; private set; } = backlogId;
    public CreateIssueByNameDto CreateIssueByNameDto { get; private set; } = createIssueByNameDto;
}
