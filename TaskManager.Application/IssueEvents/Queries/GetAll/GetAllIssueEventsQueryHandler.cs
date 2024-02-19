namespace TaskManager.Application.IssueEvents.Queries.GetAll;

internal sealed class GetAllIssueEventsQueryHandler(
    IIssueEventRepository issueEventRepository
    )
    : IQueryHandler<GetAllIssueEventsQuery, Maybe<IReadOnlyCollection<IssueEventViewModel>>>
{
    private readonly IIssueEventRepository _issueEventRepository = issueEventRepository;

    public async Task<Maybe<IReadOnlyCollection<IssueEventViewModel>>> Handle(GetAllIssueEventsQuery request, CancellationToken cancellationToken)
    {
        var issueEventViewModels = await _issueEventRepository.GetIssueEventViewModelsAsync();
        return Maybe<IReadOnlyCollection<IssueEventViewModel>>.From(issueEventViewModels);
    }
}
