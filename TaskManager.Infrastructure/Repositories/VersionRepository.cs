using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;
using Version = TaskManager.Core.Entities.Version;

namespace TaskManager.Infrastructure.Repositories;

public class VersionRepository : IVersionRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public VersionRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(_context));
    }

    public Version Add(Version version)
    {
        return _context.Versions.Add(version).Entity;
    }

    public void Delete(Guid id)
    {
        var version = _context.Versions.FirstOrDefault(v => v.Id == id);
        _context.Versions.Remove(version!);
    }

    public async Task<Version> GetById(Guid id)
    {
        var version = await _context.Versions.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
        return version!;
    }

    public async Task<IReadOnlyCollection<Version>> GetByProjectId(Guid projectId)
    {
        var versions = await _context.Versions.AsNoTracking().Where(e => e.ProjectId == projectId).ToListAsync();
        return versions;
    }

    public void Update(Version version)
    {
        _context.Entry(version).State = EntityState.Modified;
    }

    public void AddRange(IReadOnlyCollection<VersionIssue> versionIssues)
    {
        _context.VersionIssues.AddRange(versionIssues);
    }

    public async Task<IReadOnlyCollection<Guid>> GetIssueIdsByVersionId(Guid versionId)
    {
        var issueIds = await _context.VersionIssues.Where(vi => vi.VersionId == versionId).Select(vi => vi.IssueId).ToListAsync();
        return issueIds.AsReadOnly();
    }

    public void AddVersionIssue(VersionIssue versionIssue)
    {
        _context.VersionIssues.Add(versionIssue);
    }

    public void RemoveRange(IReadOnlyCollection<VersionIssue> versionIssues)
    {
        _context.VersionIssues.RemoveRange(versionIssues);
    }

    public async Task<IReadOnlyCollection<VersionIssue>> GetVersionIssuesByIssueId(Guid issueId)
    {
        var versionIssues = await _context.VersionIssues.Where(vi => vi.IssueId == issueId).ToListAsync();
        return versionIssues.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<VersionViewModel>> GetStatusViewModelsByIssueId(Guid issueId)
    {
        var versionViewModels = await (from vi in _context.VersionIssues.Where(vi => vi.IssueId == issueId)
                                       join v in _context.Versions on vi.VersionId equals v.Id
                                       select new VersionViewModel
                                       {
                                           Id = v.Id,
                                           Name = v.Name,
                                       }).ToListAsync();

        return versionViewModels.AsReadOnly();
    }
}
