namespace TaskManager.Application.Issues.Queries.GetIssuesBySprintId;

public sealed class GetIssuesBySprintIdQuery(
    Guid sprintId
    )
    : IQuery<Maybe<IReadOnlyCollection<IssueViewModel>>>
{
    public Guid SprintId { get; private set; } = sprintId;
}
