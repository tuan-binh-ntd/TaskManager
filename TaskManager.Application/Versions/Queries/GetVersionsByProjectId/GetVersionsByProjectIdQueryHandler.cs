namespace TaskManager.Application.Versions.Queries.GetVersionsByProjectId;

internal sealed class GetVersionsByProjectIdQueryHandler(
    IVersionRepository versionRepository
    )
    : IQueryHandler<GetVersionsByProjectIdQuery, Maybe<IReadOnlyCollection<VersionViewModel>>>
{
    private readonly IVersionRepository _versionRepository = versionRepository;

    public async Task<Maybe<IReadOnlyCollection<VersionViewModel>>> Handle(GetVersionsByProjectIdQuery request, CancellationToken cancellationToken)
    {
        var versions = await _versionRepository.GetVersionsByProjectIdAsync(request.ProjectId);
        return Maybe<IReadOnlyCollection<VersionViewModel>>.From(versions.Adapt<IReadOnlyCollection<VersionViewModel>>());
    }
}
