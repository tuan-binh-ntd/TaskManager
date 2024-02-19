namespace TaskManager.Application.IssueTypes.Commands.Create;

public sealed class CreateIssueTypeCommand(
    CreateIssueTypeDto createIssueTypeDto,
    Guid projectId
    )
    : ICommand<Result<IssueTypeViewModel>>
{
    public CreateIssueTypeDto CreateIssueTypeDto { get; private set; } = createIssueTypeDto;
    public Guid ProjectId { get; private set; } = projectId;
}
