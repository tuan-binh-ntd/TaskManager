namespace TaskManager.Application.Filters.Queries.GetIssuesByFilterConfiguration;

public sealed class GetIssuesByFilterConfigurationQuery(
    Guid filterId,
    Guid userId
    )
    : IQuery<Maybe<IReadOnlyCollection<IssueViewModel>>>
{
    public Guid FilterId { get; private set; } = filterId;
    public Guid UserId { get; private set; } = userId;
}
