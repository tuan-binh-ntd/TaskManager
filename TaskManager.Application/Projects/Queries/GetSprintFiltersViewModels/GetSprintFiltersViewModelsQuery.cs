namespace TaskManager.Application.Projects.Queries.GetSprintFiltersViewModels;

public sealed class GetSprintFiltersViewModelsQuery(
    Guid projectId
    )
    : IQuery<Maybe<IReadOnlyCollection<SprintFilterViewModel>>>
{
    public Guid ProjectId { get; private set; } = projectId;
}
