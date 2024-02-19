namespace TaskManager.Application.Priorities.Queries.GetPrioritiesByProjectId;

internal sealed class GetPrioritiesByProjectIdQueryHandler(
    IPriorityRepository priorityRepository
    )
    : IQueryHandler<GetPrioritiesByProjectIdQuery, Maybe<object>>
{
    private readonly IPriorityRepository _priorityRepository = priorityRepository;

    public async Task<Maybe<object>> Handle(GetPrioritiesByProjectIdQuery request, CancellationToken cancellationToken)
    {
        if (request.PaginationInput.IsPaging())
        {
            var priorities = await _priorityRepository.GetPriorityViewModelsByProjectIdPagingAsync(request.ProjectId, request.PaginationInput);
            return Maybe<PaginationResult<PriorityViewModel>>.From(priorities);
        }
        else
        {
            var priorities = await _priorityRepository.GetPriorityViewModelsByProjectIdAsync(request.ProjectId);
            return Maybe<IReadOnlyCollection<PriorityViewModel>>.From(priorities);
        }
    }
}
