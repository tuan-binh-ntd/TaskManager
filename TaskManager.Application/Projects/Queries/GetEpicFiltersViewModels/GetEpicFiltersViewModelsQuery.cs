namespace TaskManager.Application.Projects.Queries.GetEpicFiltersViewModels;

public sealed class GetEpicFiltersViewModelsQuery(
    Guid projectId
    )
    : IQuery<Maybe<IReadOnlyCollection<EpicFilterViewModel>>>
{
    public Guid ProjectId { get; private set; } = projectId;
}
