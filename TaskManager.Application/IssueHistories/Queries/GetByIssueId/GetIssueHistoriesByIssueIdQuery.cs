namespace TaskManager.Application.IssueHistories.Queries.GetByIssueId;

public sealed class GetIssueHistoriesByIssueIdQuery(
    Guid issueId
    )
    : IQuery<Maybe<IReadOnlyCollection<IssueHistoryViewModel>>>
{
    public Guid IssueId { get; private set; } = issueId;
}
