namespace TaskManager.Application.IssueTypes.Queries.GetIssueTypesByProjectId;

internal class GetIssueTypesByProjectIdQueryHandler(
    IIssueTypeRepository issueTypeRepository
    )
    : IQueryHandler<GetIssueTypesByProjectIdQuery, Maybe<object>>
{
    private readonly IIssueTypeRepository _issueTypeRepository = issueTypeRepository;

    public async Task<Maybe<object>> Handle(GetIssueTypesByProjectIdQuery request, CancellationToken cancellationToken)
    {
        if (request.PaginationInput.IsPaging())
        {
            var issueTypes = await _issueTypeRepository.GetIssueTypeViewModelsByProjectIdPagingAsync(request.ProjectId, request.PaginationInput);
            return Maybe<PaginationResult<IssueTypeViewModel>>.From(issueTypes);
        }
        else
        {
            var issueTypes = await _issueTypeRepository.GetIssueTypeViewModelsByProjectIdAsync(request.ProjectId);
            return Maybe<IReadOnlyCollection<IssueTypeViewModel>>.From(issueTypes);
        }
    }
}
