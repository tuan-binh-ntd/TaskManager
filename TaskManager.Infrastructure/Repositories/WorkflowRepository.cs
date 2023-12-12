using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
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
}
