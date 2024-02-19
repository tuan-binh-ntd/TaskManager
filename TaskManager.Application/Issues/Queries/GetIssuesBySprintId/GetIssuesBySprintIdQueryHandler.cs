namespace TaskManager.Application.Issues.Queries.GetIssuesBySprintId;

internal sealed class GetIssuesBySprintIdQueryHandler(
    IIssueRepository issueRepository
    )
    : IQueryHandler<GetIssuesBySprintIdQuery, Maybe<IReadOnlyCollection<IssueViewModel>>>
{
    private readonly IIssueRepository _issueRepository = issueRepository;

    public async Task<Maybe<IReadOnlyCollection<IssueViewModel>>> Handle(GetIssuesBySprintIdQuery request, CancellationToken cancellationToken)
    {
        var issues = await _issueRepository.GetIssuesBySprintIdAsync(request.SprintId);
        return Maybe<IReadOnlyCollection<IssueViewModel>>.From(issues.Adapt<IReadOnlyCollection<IssueViewModel>>());
    }
}
