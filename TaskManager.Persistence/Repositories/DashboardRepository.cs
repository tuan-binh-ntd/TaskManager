namespace TaskManager.Persistence.Repositories;

public class DashboardRepository(
    IDbContext context
    , IConnectionFactory connectionFactory) : GenericRepository<Issue>(context)
    , IDashboardRepository
{
    private readonly IConnectionFactory _connectionFactory = connectionFactory;

    public async Task<IReadOnlyCollection<IssueOfAssigneeDashboardViewModel>> GetIssueOfAssigneeDashboardViewModel(Guid projectId)
    {
        string query = @"
            SELECT 
              id.Id,
              IIF(id.Id IS NULL, 'Unassgined', id.[Name]) [Name],
              SUM(id.IssueCount) IssueCount
            FROM (
              SELECT
                id.AssigneeId Id,
                IIF(id.AssigneeId IS NULL, 'Unassgined', u.[Name]) [Name],
                COUNT(ISNULL(id.AssigneeId, '00000000-0000-0000-0000-000000000000')) IssueCount
              FROM Sprints s 
              JOIN Issues i ON s.Id = i.SprintId
              JOIN IssueDetails id ON i.Id = id.IssueId
              LEFT JOIN AppUser u ON id.AssigneeId = u.Id
              WHERE s.ProjectId = @ProjectId
              GROUP BY id.AssigneeId, u.[Name]
              UNION
              SELECT
                id.AssigneeId Id,
                IIF(id.AssigneeId IS NULL, 'Unassgined', u.[Name]) [Name],
                COUNT(ISNULL(id.AssigneeId, '00000000-0000-0000-0000-000000000000')) IssueCount
              FROM Backlogs b
              JOIN Issues i ON b.Id = i.BacklogId
              JOIN IssueDetails id ON i.Id = id.IssueId
              LEFT JOIN AppUser u ON id.AssigneeId = u.Id
              WHERE b.ProjectId = @ProjectId
              GROUP BY id.AssigneeId, u.[Name]
              UNION
              SELECT 
                id.AssigneeId Id,
                IIF(id.AssigneeId IS NULL, 'Unassgined', u.[Name]) [Name],
                COUNT(ISNULL(id.AssigneeId, '00000000-0000-0000-0000-000000000000')) IssueCount
              FROM Issues i
              JOIN IssueDetails id ON i.Id = id.IssueId
              LEFT JOIN AppUser u ON id.AssigneeId = u.Id
              WHERE ProjectId = @ProjectId
              GROUP BY id.AssigneeId, u.[Name]
            ) id
            GROUP BY id.Id, id.[Name]
        ";

        var param = new DynamicParameters();
        param.Add("ProjectId", projectId, DbType.Guid);

        var issueOfAssigneeDashboardViewModels = await _connectionFactory.QueryAsync<IssueOfAssigneeDashboardViewModel>(query, param);

        return issueOfAssigneeDashboardViewModels.ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<IssuesInProjectDashboardViewModel>> GetIssuesInProjectDashboardViewModelAsync(Guid projectId)
    {
        var param = new DynamicParameters();
        param.Add("ProjectId", projectId, DbType.Guid);
        string query = @"
            SELECT
              i.BacklogId Id,
              'Backlog' [Name],
              COUNT(i.BacklogId) IssueCount
            FROM Backlogs b
            JOIN Issues i ON b.Id = i.BacklogId
            WHERE b.ProjectId = @ProjectId
            GROUP BY i.BacklogId
            UNION
            SELECT
              i.SprintId Id,
              s.[Name],
              COUNT(i.SprintId) IssueCount
            FROM Sprints s
            JOIN Issues i ON s.Id = i.SprintId
            WHERE s.ProjectId = @ProjectId
            GROUP BY i.SprintId, s.[Name]
        ";

        var issuesInProjectDashboards = await _connectionFactory.QueryAsync<IssuesInProjectDashboardViewModel>(query, param);

        return issuesInProjectDashboards.ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Issue>> GetIssueViewModelsDashboardViewModelAsync(Guid projectId, GetIssuesForAssigneeOrReporterDto getIssuesForAssigneeOrReporterDto)
    {
        var sprintIds = await DbContext.Set<Sprint>()
            .AsNoTracking()
            .Where(s => s.ProjectId == projectId)
            .Select(s => s.Id)
            .ToListAsync();

        var backlogId = await DbContext.Set<Backlog>()
            .AsNoTracking()
            .Where(s => s.ProjectId == projectId)
            .Select(b => b.Id)
            .FirstOrDefaultAsync();

        var issues = await (from i in Entity
                            .WhereIf(sprintIds.Count > 0, i => sprintIds.Contains((Guid)i.SprintId!) || i.ProjectId == projectId || i.BacklogId == backlogId)
                                //.WhereIf(backlogId != Guid.Empty, i => i.BacklogId == backlogId)
                                //.Where(i => i.ProjectId == projectId)
                            join id in DbContext.Set<IssueDetail>()
                            .Where(i => i.CreationTime >= getIssuesForAssigneeOrReporterDto.StartDate && i.CreationTime <= getIssuesForAssigneeOrReporterDto.EndDate)
                            .WhereIf(getIssuesForAssigneeOrReporterDto.Type == DashboardConstants.AssigneeType, id => id.AssigneeId == getIssuesForAssigneeOrReporterDto.UserId)
                            .WhereIf(getIssuesForAssigneeOrReporterDto.Type == DashboardConstants.ReporterType, id => id.ReporterId == getIssuesForAssigneeOrReporterDto.UserId)
                            .WhereIf(getIssuesForAssigneeOrReporterDto.Type == DashboardConstants.AllType || getIssuesForAssigneeOrReporterDto.Type == "all", id => id.AssigneeId == getIssuesForAssigneeOrReporterDto.UserId || id.ReporterId == getIssuesForAssigneeOrReporterDto.UserId)
                            on i.Id equals id.IssueId
                            select i)
                            .ToListAsync();

        return issues.AsReadOnly();
    }
}
