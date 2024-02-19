namespace TaskManager.Application.IssueHistories.Queries.GetByIssueId;

internal sealed class GetIssueHistoriesByIssueIdQueryHandler(
    IIssueHistoryRepository issueHistoryRepository
    )
    : IQueryHandler<GetIssueHistoriesByIssueIdQuery, Maybe<IReadOnlyCollection<IssueHistoryViewModel>>>
{
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;

    public async Task<Maybe<IReadOnlyCollection<IssueHistoryViewModel>>> Handle(GetIssueHistoriesByIssueIdQuery request, CancellationToken cancellationToken)
    {
        var issueHistories = await _issueHistoryRepository.GetIssueHistoriesByIssueIdAsync(request.IssueId);
        return Maybe<IReadOnlyCollection<IssueHistoryViewModel>>.From(issueHistories.Adapt<IReadOnlyCollection<IssueHistoryViewModel>>());
    }
}
