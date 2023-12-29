using Microsoft.EntityFrameworkCore;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Extensions;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Project Add(Project project)
    {
        return _context.Projects.Add(project).Entity;
    }

    public void Update(Project project)
    {
        _context.Entry(project).State = EntityState.Modified;
    }

    public void Delete(Project project)
    {
        _context.Projects.Remove(project);
    }

    public async Task<IReadOnlyCollection<Project>> GetAll()
    {
        var projects = await _context.Projects
            .AsNoTracking()
            .Include(p => p.UserProjects!)
            .ThenInclude(up => up.User)
            .ToListAsync();
        return projects.AsReadOnly();
    }

    public async Task<Project?> GetById(Guid id)
    {
        var project = await _context.Projects
            .Include(p => p.UserProjects!)
            .ThenInclude(up => up.User)
            .SingleOrDefaultAsync(p => p.Id == id);
        return project;
    }

    public async Task<PaginationResult<Project>> GetByUserId(Guid userId, GetProjectByFilterDto filter, PaginationInput paginationInput)
    {

        var query = from u in _context.Users.AsNoTracking()
                    join up in _context.UserProjects.AsNoTracking().Where(up => up.UserId == userId) on u.Id equals up.UserId
                    join p in _context.Projects.AsNoTracking()
                    .WhereIf(!string.IsNullOrWhiteSpace(filter.name), p => p.Name.Contains(filter.name))
                    .WhereIf(!string.IsNullOrWhiteSpace(filter.code), p => p.Code.Contains(filter.code))
                    on up.ProjectId equals p.Id
                    select p;

        return await query.Pagination(paginationInput);
    }

    public async Task<IReadOnlyCollection<Project>> GetByUserId(Guid userId, GetProjectByFilterDto filter)
    {
        var query = from u in _context.Users.AsNoTracking()
                    join up in _context.UserProjects.AsNoTracking().Where(up => up.UserId == userId) on u.Id equals up.UserId
                    join p in _context.Projects.AsNoTracking()
                    .WhereIf(!string.IsNullOrWhiteSpace(filter.name), p => p.Name.Contains(filter.name))
                    .WhereIf(!string.IsNullOrWhiteSpace(filter.code), p => p.Code.Contains(filter.code))
                    on up.ProjectId equals p.Id
                    select p;

        var projects = await query.ToListAsync();
        return projects.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<UserViewModel>> GetMembers(Guid projectId)
    {
        var members = await (from up in _context.UserProjects.AsNoTracking().Where(up => up.ProjectId == projectId)
                             join u in _context.Users.AsNoTracking() on up.UserId equals u.Id
                             select new UserViewModel
                             {
                                 Id = u.Id,
                                 Name = u.Name,
                                 Department = u.Department,
                                 Organization = u.Organization,
                                 AvatarUrl = u.AvatarUrl,
                                 JobTitle = u.JobTitle,
                                 Location = u.Location,
                                 Email = u.Email,
                                 Role = up.Role
                             }).ToListAsync();
        return members.AsReadOnly();
    }

    public async Task<Project?> GetByCode(string code)
    {
        var project = await _context.Projects.AsNoTracking().SingleOrDefaultAsync(p => p.Code == code);
        return project;
    }

    public async Task LoadIssueTypes(Project project)
    {
        await _context.Entry(project).Collection(p => p.IssueTypes!).LoadAsync();
    }

    public async Task LoadStatuses(Project project)
    {
        await _context.Entry(project).Collection(p => p.Statuses!).LoadAsync();
    }

    public async Task LoadBacklog(Project project)
    {
        await _context.Entry(project).Reference(p => p.Backlog).LoadAsync();
    }

    public async Task LoadUserProjects(Project project)
    {
        await _context.Entry(project).Collection(p => p.UserProjects!).LoadAsync();
    }

    public async Task LoadProjectConfiguration(Project project)
    {
        await _context.Entry(project).Reference(p => p.ProjectConfiguration).LoadAsync();
    }

    public async Task LoadTransition(Project project)
    {
        await _context.Entry(project).Collection(p => p.Transitions!).LoadAsync();
    }

    public async Task LoadWorkflow(Project project)
    {
        await _context.Entry(project).Reference(p => p.Workflow).LoadAsync();
    }

    public async Task LoadPriorities(Project project)
    {
        await _context.Entry(project).Collection(p => p.Priorities!).LoadAsync();
    }

    public async Task LoadPermissionGroup(Project project)
    {
        await _context.Entry(project).Collection(p => p.PermissionGroups!).LoadAsync();
    }

    public async Task LoadSprints(Project project)
    {
        await _context.Entry(project).Collection(p => p.Sprints!).LoadAsync();
    }

    public async Task LoadVersions(Project project)
    {
        await _context.Entry(project).Collection(p => p.Versions!).LoadAsync();
    }

    public async Task<string> GetProjectName(Guid projectId)
    {
        var name = await _context.Projects.Where(p => p.Id == projectId).Select(p => p.Name).FirstOrDefaultAsync();
        return name!;
    }

    public async Task<IReadOnlyCollection<SprintFilterViewModel>> GetSprintFiltersByProjectId(Guid projectId)
    {
        var sprintFilterViewModels = await _context.Sprints
            .AsNoTracking()
            .Where(s => s.ProjectId == projectId && s.IsStart == true)
            .Select(s => new SprintFilterViewModel
            {
                Id = s.Id,
                Name = s.Name
            }).ToListAsync();

        return sprintFilterViewModels.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<EpicFilterViewModel>> GetEpicFiltersByProjectId(Guid projectId)
    {
        var epicFilterViewModels = await (from s in _context.Sprints.Where(s => s.ProjectId == projectId && s.IsStart == true)
                                          join i in _context.Issues on s.Id equals i.SprintId
                                          join e in _context.Issues on i.ParentId equals e.Id
                                          group e by new { e.Id, e.Name } into g
                                          select new EpicFilterViewModel
                                          {
                                              Id = g.Key.Id,
                                              Name = g.Key.Name,
                                          }).ToArrayAsync();

        return epicFilterViewModels.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<LabelFilterViewModel>> GetLabelFiltersByProjectId(Guid projectId)
    {
        var epicFilterViewModels = await (from s in _context.Sprints.Where(s => s.ProjectId == projectId && s.IsStart == true)
                                          join i in _context.Issues on s.Id equals i.SprintId
                                          join li in _context.LabelIssues on i.Id equals li.IssueId
                                          join l in _context.Labels on li.LabelId equals l.Id
                                          group l by new { l.Id, l.Name } into g
                                          select new LabelFilterViewModel
                                          {
                                              Id = g.Key.Id,
                                              Name = g.Key.Name,
                                          }).ToArrayAsync();

        return epicFilterViewModels.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<TypeFilterViewModel>> GetIssueTypeFiltersByProjectId(Guid projectId)
    {
        var epicFilterViewModels = await (from s in _context.Sprints.Where(s => s.ProjectId == projectId && s.IsStart == true)
                                          join i in _context.Issues on s.Id equals i.SprintId
                                          join it in _context.IssueTypes on i.IssueTypeId equals it.Id
                                          group it by new { it.Id, it.Name } into g
                                          select new TypeFilterViewModel
                                          {
                                              Id = g.Key.Id,
                                              Name = g.Key.Name,
                                          }).ToArrayAsync();

        return epicFilterViewModels.AsReadOnly();
    }

    public async Task<Guid> GetUserIdByMemberId(Guid memberId)
    {
        var userId = await _context.UserProjects.Where(up => up.Id == memberId).Select(up => up.UserId).FirstOrDefaultAsync();
        return userId;
    }
}