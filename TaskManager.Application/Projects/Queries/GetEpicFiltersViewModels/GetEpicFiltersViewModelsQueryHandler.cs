namespace TaskManager.Application.Projects.Queries.GetEpicFiltersViewModels;

internal sealed class GetEpicFiltersViewModelsQueryHandler(
    IProjectRepository projectRepository
    )
    : IQueryHandler<GetEpicFiltersViewModelsQuery, Maybe<IReadOnlyCollection<EpicFilterViewModel>>>
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<Maybe<IReadOnlyCollection<EpicFilterViewModel>>> Handle(GetEpicFiltersViewModelsQuery request, CancellationToken cancellationToken)
    {
        var epicFilterViewModels = await _projectRepository.GetEpicFiltersByProjectIdAsync(request.ProjectId);
        return Maybe<IReadOnlyCollection<EpicFilterViewModel>>.From(epicFilterViewModels);
    }
}
