using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
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
}
