namespace TaskManager.Application.Issues.Queries.GetIssuesByBacklogId;

internal sealed class GetIssuesByBacklogIdQueryHandler(
    IIssueRepository issueRepository
    )
    : IQueryHandler<GetIssuesByBacklogIdQuery, Maybe<IReadOnlyCollection<IssueViewModel>>>
{
    private readonly IIssueRepository _issueRepository = issueRepository;

    public async Task<Maybe<IReadOnlyCollection<IssueViewModel>>> Handle(GetIssuesByBacklogIdQuery request, CancellationToken cancellationToken)
    {
        var issues = await _issueRepository.GetIssuesByBacklogIdAsync(request.BacklogId);
        return Maybe<IReadOnlyCollection<IssueViewModel>>.From(issues.Adapt<IReadOnlyCollection<IssueViewModel>>());
    }
}
