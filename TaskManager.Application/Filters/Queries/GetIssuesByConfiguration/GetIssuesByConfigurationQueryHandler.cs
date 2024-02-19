namespace TaskManager.Application.Filters.Queries.GetIssuesByConfiguration;

internal sealed class GetIssuesByConfigurationQueryHandler(
    IBacklogRepository backlogRepository,
    ISprintRepository sprintRepository,
    ILogger<GetIssuesByConfigurationQueryHandler> logger,
    IConnectionFactory connectionFactory,
    IIssueRepository issueRepository
    )
    : IQueryHandler<GetIssuesByConfigurationQuery, Maybe<IReadOnlyCollection<IssueViewModel>>>
{
    private readonly IBacklogRepository _backlogRepository = backlogRepository;
    private readonly ISprintRepository _sprintRepository = sprintRepository;
    private readonly ILogger<GetIssuesByConfigurationQueryHandler> _logger = logger;
    private readonly IConnectionFactory _connectionFactory = connectionFactory;
    private readonly IIssueRepository _issueRepository = issueRepository;

    public async Task<Maybe<IReadOnlyCollection<IssueViewModel>>> Handle(GetIssuesByConfigurationQuery request, CancellationToken cancellationToken)
    {
        FilterConfiguration filterConfiguration = new()
        {
            Project = request.GetIssueByConfigurationDto.Project,
            Type = request.GetIssueByConfigurationDto.Type,
            Status = request.GetIssueByConfigurationDto.Status,
            Assginee = request.GetIssueByConfigurationDto.Assginee,
            Created = request.GetIssueByConfigurationDto.Created,
            DueDate = request.GetIssueByConfigurationDto.DueDate,
            FixVersions = request.GetIssueByConfigurationDto.FixVersions,
            Labels = request.GetIssueByConfigurationDto.Labels,
            Priority = request.GetIssueByConfigurationDto.Priority,
            Reporter = request.GetIssueByConfigurationDto.Reporter,
            Resolution = request.GetIssueByConfigurationDto.Resolution,
            Resolved = request.GetIssueByConfigurationDto.Resolved,
            Sprints = request.GetIssueByConfigurationDto.Sprints,
            StatusCategory = request.GetIssueByConfigurationDto.StatusCategory,
            Updated = request.GetIssueByConfigurationDto.Updated,
        };
        if (request.GetIssueByConfigurationDto.Project is not null
            && request.GetIssueByConfigurationDto.Project.ProjectIds is not null
            && request.GetIssueByConfigurationDto.Project.ProjectIds.Count != 0)
        {
            filterConfiguration.Project!.BacklogIds = await _backlogRepository.GetBacklogIdsByProjectIdsAsync(request.GetIssueByConfigurationDto.Project.ProjectIds);
            filterConfiguration.Project.SprintIds = await _sprintRepository.GetSprintIdsByProjectIdsAsync(request.GetIssueByConfigurationDto.Project.ProjectIds);
        }
        string query = filterConfiguration.QueryAfterBuild();
        _logger.LogInformation(query);
        var issueIds = await _connectionFactory.QueryAsync<Guid>(query);

        var issues = await _issueRepository.GetIssuesByIdsAsync([.. issueIds]);
        return Maybe<IReadOnlyCollection<IssueViewModel>>.From(issues.Adapt<IReadOnlyCollection<IssueViewModel>>());
    }
}
