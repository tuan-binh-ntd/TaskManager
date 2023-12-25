using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Extensions;
using TaskManager.Core.Helper;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class LabelRepository : ILabelRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public LabelRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Add(Label label)
    {
        _context.Labels.Add(label);
    }

    public void Delete(Label label)
    {
        _context.Labels.Remove(label);
    }

    public async Task<Label?> GetById(Guid id)
    {
        var query = _context.Labels.Where(x => x.Id == id).Select(l => l);
        return await query.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<Label>> GetByProjectId(Guid projectId)
    {
        var labels = await _context.Labels.Where(l => l.ProjectId == projectId).ToListAsync();
        return labels.AsReadOnly();
    }

    public void Update(Label label)
    {
        _context.Entry(label).State = EntityState.Modified;
    }

    public void AddLabelIssue(LabelIssue labelIssue)
    {
        _context.LabelIssues.Add(labelIssue);
    }

    public void RemoveLabelIssue(LabelIssue labelIssue)
    {
        _context.LabelIssues.Remove(labelIssue);
    }

    public async Task<LabelIssue?> GetById(Guid labelId, Guid issueId)
    {
        var query = _context.LabelIssues.Where(x => x.IssueId == issueId && x.LabelId == labelId).Select(l => l);
        return await query.FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<LabelViewModel>> GetByIssueId(Guid issueId)
    {
        var labels = await (from li in _context.LabelIssues.Where(li => li.IssueId == issueId)
                            join l in _context.Labels on li.LabelId equals l.Id
                            select new LabelViewModel
                            {
                                Id = l.Id,
                                Name = l.Name,
                                Color = l.Color,
                                Description = l.Description,
                            }).ToListAsync();

        return labels.AsReadOnly();
    }

    public async Task<PaginationResult<LabelViewModel>> GetByProjectId(Guid projectId, PaginationInput paginationInput)
    {
        var labels = await (from l in _context.Labels.Where(l => l.ProjectId == projectId)
                            select new LabelViewModel
                            {
                                Id = l.Id,
                                Name = l.Name,
                                Color = l.Color,
                                Description = l.Description,
                            }).Pagination(paginationInput);
        return labels;
    }

    public void AddRange(IReadOnlyCollection<LabelIssue> labelIssues)
    {
        _context.LabelIssues.AddRange(labelIssues);
    }

    public async Task<IReadOnlyCollection<LabelIssue>> GetLabelIssuesByIssueId(Guid issueId)
    {
        var labels = await _context.LabelIssues.Where(li => li.IssueId == issueId).ToListAsync();
        return labels.AsReadOnly();
    }

    public void RemoveRange(IReadOnlyCollection<LabelIssue> labelIssues)
    {
        _context.LabelIssues.RemoveRange(labelIssues);
    }

    public async Task<IReadOnlyCollection<LabelIssue>> GetLabelIssuesByLabelId(Guid labelId)
    {
        var labelIssues = await _context.LabelIssues.Where(li => li.LabelId == labelId).ToListAsync();
        return labelIssues.AsReadOnly();
    }
}
