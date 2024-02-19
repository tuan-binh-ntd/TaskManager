namespace TaskManager.Application.Statuses.Queries.GetStatusesByProjectId;

internal sealed class GetStatusesByProjectIdQueryHandler(
    IStatusRepository statusRepository
    )
     : IQueryHandler<GetStatusesByProjectIdQuery, Maybe<object>>
{
    private readonly IStatusRepository _statusRepository = statusRepository;

    public async Task<Maybe<object>> Handle(GetStatusesByProjectIdQuery request, CancellationToken cancellationToken)
    {
        if (request.PaginationInput.IsPaging())
        {
            var statuses = await _statusRepository.GetStatusViewModelsByProjectIdPagingAsync(request.ProjectId, request.PaginationInput);
            return Maybe<PaginationResult<StatusViewModel>>.From(statuses);
        }
        else
        {
            var statuses = await _statusRepository.GetStatusViewModelsByProjectIdAsync(request.ProjectId);
            return Maybe<IReadOnlyCollection<StatusViewModel>>.From(statuses);
        }
    }
}
