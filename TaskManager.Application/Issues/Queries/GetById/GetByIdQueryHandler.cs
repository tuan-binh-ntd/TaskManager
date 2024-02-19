namespace TaskManager.Application.Issues.Queries.GetById;

internal sealed class GetByIdQueryHandler(
    IIssueRepository issueRepository
    )
    : IQueryHandler<GetByIdQuery, Maybe<IssueViewModel>>
{
    private readonly IIssueRepository _issueRepository = issueRepository;

    public async Task<Maybe<IssueViewModel>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var issue = await _issueRepository.GetByIdAsync(request.IssueId);
        return Maybe<IssueViewModel>.From(issue.Adapt<IssueViewModel>());
    }
}
