using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class IssueRepository : IIssueRepository
    {
        private readonly AppDbContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public IssueRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Issue Add(Issue issue)
        {
            return _context.Issues.Add(issue).Entity;
        }

        public void Delete(Guid id)
        {
            var issue = _context.Issues.SingleOrDefault(e => e.Id == id);
            _context.Issues.Remove(issue!);
        }

        public void Update(Issue issue)
        {
            _context.Entry(issue).State = EntityState.Modified;
        }

        public async Task<Issue> Get(Guid id)
        {
            var issue = await _context.Issues.FirstOrDefaultAsync(e => e.Id == id);
            return issue!;
        }

        public async Task<IReadOnlyCollection<Issue>> GetIssueBySprintId(Guid sprintId)
        {
            var issues = await _context.Issues
                .Where(i => i.SprintId == sprintId)
                .Include(i => i.Sprint)
                .Include(i => i.IssueType)
                .Include(i => i.IssueDetail)
                .Include(i => i.IssueHistories)
                .Include(i => i.Comments)
                .Include(i => i.Attachments)
                .ToListAsync();
            return issues.AsReadOnly();
        }

        public async Task<IReadOnlyCollection<Issue>> GetIssueByBacklogId(Guid backlogId)
        {
            var issues = await _context.Issues
                .Where(i => i.BacklogId == backlogId)
                .Include(i => i.Backlog)
                .Include(i => i.IssueType)
                .Include(i => i.IssueDetail)
                .Include(i => i.IssueHistories)
                .Include(i => i.Comments)
                .Include(i => i.Attachments)
                .ToListAsync();
            return issues.AsReadOnly();
        }

        public void UpdateRange(IReadOnlyCollection<Issue> issues)
        {
            _context.Issues.UpdateRange(issues);
        }

        public async Task<IReadOnlyCollection<Issue>> GetChildIssues(Guid parentId)
        {
            var childIssues = await _context.Issues
                .Where(i => i.ParentId == parentId)
                .Include(i => i.Backlog)
                .Include(i => i.IssueType)
                .Include(i => i.IssueDetail)
                .Include(i => i.IssueHistories)
                .Include(i => i.Comments)
                .Include(i => i.Attachments)
                .ToListAsync();
            return childIssues.AsReadOnly();
        }

        public void DeleteRange(IReadOnlyCollection<Issue> issues)
        {
            _context.Issues.RemoveRange(issues);
        }

        public void LoadEntitiesRelationship(Issue issue)
        {
            _context.Entry(issue).Reference(i => i.IssueType).Load();
            _context.Entry(issue).Reference(i => i.IssueDetail).Load();
            _context.Entry(issue).Collection(i => i.Attachments!).Load();
            _context.Entry(issue).Collection(i => i.IssueHistories!).Load();
            _context.Entry(issue).Collection(i => i.Comments!).Load();
            _context.Entry(issue).Reference(i => i.Status).Load();
        }

        public async Task<IReadOnlyCollection<Issue>> GetByIds(IReadOnlyCollection<Guid> ids)
        {
            var issues = await _context.Issues.Where(e => ids.Contains(e.Id)).ToListAsync();
            return issues.AsReadOnly();
        }

        public async Task<IReadOnlyCollection<Issue>> GetEpicByProjectId(Guid projectId)
        {
            var epics = await _context.Issues.Where(e => e.SprintId == null && e.BacklogId == null && e.ProjectId == projectId).ToListAsync();
            return epics.AsReadOnly();
        }

        public async Task<IReadOnlyCollection<Issue>> GetChildIssueOfEpic(Guid epicId)
        {
            var childIssues = await _context.Issues.Where(e => e.ParentId == epicId).ToListAsync();
            return childIssues.AsReadOnly();
        }

        public async Task<IReadOnlyCollection<Issue>> GetChildIssueOfIssue(Guid issueId)
        {
            var epics = await _context.Issues.Where(e => e.ParentId == issueId).ToListAsync();
            return epics.AsReadOnly();
        }

        public void LoadComments(Issue issue)
        {
            _context.Entry(issue).Collection(i => i.Comments!).LoadAsync();
        }

        public void LoadIssueType(Issue issue)
        {
            _context.Entry(issue).Reference(i => i.IssueType).LoadAsync();
        }

        public void LoadIssueDetail(Issue issue)
        {
            _context.Entry(issue).Reference(i => i.IssueDetail).LoadAsync();
        }

        public void LoadAttachments(Issue issue)
        {
            _context.Entry(issue).Collection(i => i.Attachments!).LoadAsync();
        }

        public void LoadIssueHistories(Issue issue)
        {
            _context.Entry(issue).Collection(i => i.IssueHistories!).LoadAsync();
        }

        public void LoadStatus(Issue issue)
        {
            _context.Entry(issue).Reference(i => i.Status).LoadAsync();
        }

        public async Task<string> GetParentName(Guid parentId)
        {
            var parentName = await _context.Issues.AsNoTracking().Where(i => i.Id == parentId).Select(i => i.Name).FirstOrDefaultAsync();
            if (string.IsNullOrWhiteSpace(parentName))
            {
                return string.Empty;
            }
            return parentName;
        }

        public async Task<IReadOnlyCollection<Issue>> GetCreatedAWeekAgo()
        {
            var issues = await _context.Issues.Where(i => i.CreationTime >= DateTime.Now.AddDays(-7)).ToListAsync();
            return issues.AsReadOnly();
        }

        public async Task<IReadOnlyCollection<Issue>> GetResolvedAWeekAgo()
        {
            var issues = await _context.Issues.Where(i => i.CompleteDate >= DateTime.Now.AddDays(-7)).ToListAsync();
            return issues.AsReadOnly();
        }

        public async Task<IReadOnlyCollection<Issue>> GetUpdatedAWeekAgo()
        {
            var issues = await _context.Issues.Where(i => i.ModificationTime >= DateTime.Now.AddDays(-7)).ToListAsync();
            return issues.AsReadOnly();
        }
    }
}
