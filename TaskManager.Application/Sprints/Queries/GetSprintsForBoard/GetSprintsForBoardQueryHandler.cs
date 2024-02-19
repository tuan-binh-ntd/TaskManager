namespace TaskManager.Application.Sprints.Queries.GetSprintsForBoard;

internal sealed class GetSprintsForBoardQueryHandler(
    ISprintRepository sprintRepository,
    IIssueRepository issueRepository,
    IConnectionFactory connectionFactory,
    IStatusRepository statusRepository,
    ILogger<GetSprintsForBoardQueryHandler> logger
    )
    : IQueryHandler<GetSprintsForBoardQuery, Maybe<Dictionary<string, IReadOnlyCollection<IssueViewModel>>>>
{
    private readonly ISprintRepository _sprintRepository = sprintRepository;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IConnectionFactory _connectionFactory = connectionFactory;
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly ILogger<GetSprintsForBoardQueryHandler> _logger = logger;

    public async Task<Maybe<Dictionary<string, IReadOnlyCollection<IssueViewModel>>>> Handle(GetSprintsForBoardQuery request, CancellationToken cancellationToken)
    {
        if (request.GetSprintByFilterDto.SprintIds.Count == 0)
        {
            request.GetSprintByFilterDto.SprintIds = await _sprintRepository.GetSprintIdsByProjectIdAsync(request.ProjectId);
        }

        string query = request.GetSprintByFilterDto.FullQuery();
        if (query.Equals(string.Empty))
        {
            return Maybe<Dictionary<string, IReadOnlyCollection<IssueViewModel>>>.From([]);
        }

        _logger.LogInformation(query);
        var issueIds = await _connectionFactory.QueryAsync<Guid>(query);
        var issues = await _issueRepository.GetIssuesByIdsAsync(issueIds);
        var issueViewModels = issues.Adapt<IReadOnlyCollection<IssueViewModel>>();
        var issueOnBoard = new Dictionary<string, IReadOnlyCollection<IssueViewModel>>();
        var statuses = await _statusRepository.GetStatusesByProjectIdAsync(request.ProjectId);

        foreach (var status in statuses)
        {
            var issueByStatusIds = issueViewModels.Where(i => i.StatusId == status.Id).ToList();
            issueOnBoard.Add(status.Name, issueByStatusIds);
        }

        return Maybe<Dictionary<string, IReadOnlyCollection<IssueViewModel>>>.From(issueOnBoard);
    }
}
