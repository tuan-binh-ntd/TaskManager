namespace TaskManager.Application.Versions.Queries.GetVersionsByProjectId;

public sealed class GetVersionsByProjectIdQuery(
    Guid projectId
    )
    : IQuery<Maybe<IReadOnlyCollection<VersionViewModel>>>
{
    public Guid ProjectId { get; private set; } = projectId;
}
