namespace TaskManager.Application.StatusCategories.Queries.GetAll;

internal sealed class GetAllStatusCategoriesQueryHandler(
    IStatusCategoryRepository statusCategoryRepository
    )
    : IQueryHandler<GetAllStatusCategoriesQuery, Maybe<IReadOnlyCollection<StatusCategoryViewModel>>>
{
    private readonly IStatusCategoryRepository _statusCategoryRepository = statusCategoryRepository;

    public async Task<Maybe<IReadOnlyCollection<StatusCategoryViewModel>>> Handle(GetAllStatusCategoriesQuery request, CancellationToken cancellationToken)
    {
        var statusCategories = await _statusCategoryRepository.GetStatusCategorysAsync();
        return Maybe<IReadOnlyCollection<StatusCategoryViewModel>>.From(statusCategories.Adapt<IReadOnlyCollection<StatusCategoryViewModel>>());
    }
}
