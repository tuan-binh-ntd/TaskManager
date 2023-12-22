using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.ViewModel;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class WorkflowRepository : IWorkflowRepository
{
    private readonly AppDbContext _context;
    public IUnitOfWork UnitOfWork => _context;

    public WorkflowRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Workflow Add(Workflow workflow)
    {
        return _context.Workflows.Add(workflow).Entity;
    }

    public void Delete(Guid id)
    {
        var workflow = _context.Workflows.FirstOrDefault(x => x.Id == id);
        _context.Workflows.Remove(workflow!);
    }

    public void Update(Workflow workflow)
    {
        _context.Entry(workflow).State = EntityState.Modified;
    }

    public async Task<WorkflowViewModel> GetByProjectId(Guid projectId)
    {
        var workflowViewModel = await _context.Workflows
            .AsNoTracking()
            .Where(w => w.ProjectId == projectId)
            .Select(w => new WorkflowViewModel
            {
                Id = w.Id,
                Name = w.Name,
            })
            .FirstOrDefaultAsync();

        return workflowViewModel!;
    }

    public async Task<IReadOnlyCollection<WorkflowIssueType>> GetWorkflowIssueTypesByWorkflowId(Guid workflowId)
    {
        var workflowIssueTypes = await _context.WorkflowIssueTypes.Where(wit => wit.WorkflowId == workflowId).ToListAsync();
        return workflowIssueTypes;
    }

    public void AddWorkflowIssueTypeRange(IReadOnlyCollection<WorkflowIssueType> workflowIssueTypes)
    {
        _context.WorkflowIssueTypes.AddRange(workflowIssueTypes);
    }

    public void RemoveWorkflowIssueTypeRange(IReadOnlyCollection<WorkflowIssueType> workflowIssueTypes)
    {
        _context.WorkflowIssueTypes.RemoveRange(workflowIssueTypes);

    }
}
