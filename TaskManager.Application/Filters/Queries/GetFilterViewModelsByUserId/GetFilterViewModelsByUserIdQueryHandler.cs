namespace TaskManager.Application.Filters.Queries.GetFilterViewModelsByUserId;

internal class GetFilterViewModelsByUserIdQueryHandler(
    IFilterRepository filterRepository
    )
    : IQueryHandler<GetFilterViewModelsByUserIdQuery, Maybe<IReadOnlyCollection<FilterViewModel>>>
{
    private readonly IFilterRepository _filterRepository = filterRepository;

    public async Task<Maybe<IReadOnlyCollection<FilterViewModel>>> Handle(GetFilterViewModelsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var filterViewModels = await _filterRepository.GetFiltersByUserIdAsync(request.UserId);
        return Maybe<IReadOnlyCollection<FilterViewModel>>.From(filterViewModels);
    }
}
