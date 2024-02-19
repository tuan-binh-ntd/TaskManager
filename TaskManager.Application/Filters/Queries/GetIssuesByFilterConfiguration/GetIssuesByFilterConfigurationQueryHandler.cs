namespace TaskManager.Application.Filters.Queries.GetIssuesByFilterConfiguration;

internal sealed class GetIssuesByFilterConfigurationQueryHandler(
    IFilterRepository filterRepository,
    IUserProjectRepository userProjectRepository,
    ISprintRepository sprintRepository,
    IBacklogRepository backlogRepository,
    ILogger<GetIssuesByFilterConfigurationQueryHandler> logger,
    IConnectionFactory connectionFactory,
    IIssueRepository issueRepository
    )
    : IQueryHandler<GetIssuesByFilterConfigurationQuery, Maybe<IReadOnlyCollection<IssueViewModel>>>
{
    private readonly IFilterRepository _filterRepository = filterRepository;
    private readonly IUserProjectRepository _userProjectRepository = userProjectRepository;
    private readonly ISprintRepository _sprintRepository = sprintRepository;
    private readonly IBacklogRepository _backlogRepository = backlogRepository;
    private readonly ILogger<GetIssuesByFilterConfigurationQueryHandler> _logger = logger;
    private readonly IConnectionFactory _connectionFactory = connectionFactory;
    private readonly IIssueRepository _issueRepository = issueRepository;

    public async Task<Maybe<IReadOnlyCollection<IssueViewModel>>> Handle(GetIssuesByFilterConfigurationQuery request, CancellationToken cancellationToken)
    {
        var filterConfiguration = await _filterRepository.GetConfigurationOfFilterAsync(request.FilterId);
        var projectIds = await _userProjectRepository.GetProjectIdsByUserIdAsync(request.UserId);
        var sprintIds = await _sprintRepository.GetSprintIdsByProjectIdsAsync(projectIds);
        var backlogIds = await _backlogRepository.GetBacklogIdsByProjectIdsAsync(projectIds);

        if (filterConfiguration is not null)
        {
            filterConfiguration.Project ??= new()
            {
                BacklogIds = backlogIds,
                SprintIds = sprintIds,
                ProjectIds = projectIds
            };

            string query = filterConfiguration.QueryAfterBuild();
            _logger.LogInformation(message: query);
            var issueIds = await _connectionFactory.QueryAsync<Guid>(query);
            if (issueIds.Count != 0)
            {
                var issues = await _issueRepository.GetIssuesByIdsAsync(issueIds.ToList());
                return Maybe<IReadOnlyCollection<IssueViewModel>>.From(issues.Adapt<IReadOnlyCollection<IssueViewModel>>());
            }
            return Maybe<IReadOnlyCollection<IssueViewModel>>.From(new List<IssueViewModel>());
        }
        else
        {
            filterConfiguration = new FilterConfiguration
            {
                Project = new()
                {
                    BacklogIds = backlogIds,
                    SprintIds = sprintIds,
                    ProjectIds = projectIds
                }
            };
            string query = filterConfiguration.QueryAfterBuild();
            _logger.LogInformation(message: query);
            var issueIds = await _connectionFactory.QueryAsync<Guid>(query);
            if (issueIds.Count != 0)
            {
                var issues = await _issueRepository.GetIssuesByIdsAsync(issueIds.ToList());
                return Maybe<IReadOnlyCollection<IssueViewModel>>.From(issues.Adapt<IReadOnlyCollection<IssueViewModel>>());
            }
            return Maybe<IReadOnlyCollection<IssueViewModel>>.From(new List<IssueViewModel>());
        }
    }
}
