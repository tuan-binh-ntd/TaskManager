namespace TaskManager.Application.Sprints.Queries.GetSprintsForBoard;

public sealed class GetSprintsForBoardQuery(
    Guid projectId,
    GetSprintByFilterDto getSprintByFilterDto
    )
    : IQuery<Maybe<Dictionary<string, IReadOnlyCollection<IssueViewModel>>>>
{
    public Guid ProjectId { get; private set; } = projectId;
    public GetSprintByFilterDto GetSprintByFilterDto { get; private set; } = getSprintByFilterDto;
}
