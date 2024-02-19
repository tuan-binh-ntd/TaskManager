namespace TaskManager.Application.Issues.Queries.GetIssuesByBacklogId;

public sealed class GetIssuesByBacklogIdQuery(
    Guid backlogId
    )
     : IQuery<Maybe<IReadOnlyCollection<IssueViewModel>>>
{
    public Guid BacklogId { get; private set; } = backlogId;
}
