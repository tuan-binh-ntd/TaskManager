using Dapper;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Core;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Extensions;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly AppDbContext _context;
    private readonly IConnectionFactory _connectionFactory;

    public DashboardRepository(
        AppDbContext context,
        IConnectionFactory connectionFactory
        )
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyCollection<IssueOfAssigneeDashboardViewModel>> GetIssueOfAssigneeDashboardViewModel(Guid projectId)
    {
        var query = from up in _context.UserProjects.Where(up => up.ProjectId == projectId)
                    join u in _context.Users on up.UserId equals u.Id
                    join id in _context.IssueDetails on up.UserId equals id.AssigneeId into idj
                    from idlj in idj.DefaultIfEmpty()
                    group new { idlj, u } by new { idlj.AssigneeId, u.Id, u.Name } into g
                    select new IssueOfAssigneeDashboardViewModel
                    {
                        Id = g.Key.Id,
                        Name = g.Key.Name,
                        IssueCount = g.Count()
                    };

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyCollection<IssuesInProjectDashboardViewModel>> GetIssuesInProjectDashboardViewModelAsync(Guid projectId)
    {
        var param = new DynamicParameters();
        param.Add("ProjectId", projectId, System.Data.DbType.Guid);
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
        var issues = await (from i in _context.Issues
                            join id in _context.IssueDetails
                            .Where(i => i.CreationTime >= getIssuesForAssigneeOrReporterDto.StartDate && i.CreationTime <= getIssuesForAssigneeOrReporterDto.EndDate)
                            .WhereIf(getIssuesForAssigneeOrReporterDto.Type == CoreConstants.AssigneeType, id => id.AssigneeId == getIssuesForAssigneeOrReporterDto.UserId)
                            .WhereIf(getIssuesForAssigneeOrReporterDto.Type == CoreConstants.ReporterType, id => id.ReporterId == getIssuesForAssigneeOrReporterDto.UserId)
                            .WhereIf(getIssuesForAssigneeOrReporterDto.Type == CoreConstants.AllType, id => id.AssigneeId == getIssuesForAssigneeOrReporterDto.UserId || id.ReporterId == getIssuesForAssigneeOrReporterDto.UserId)
                            on i.Id equals id.IssueId
                            select i).ToListAsync();

        return issues.AsReadOnly();
    }
}
