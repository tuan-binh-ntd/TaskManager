namespace TaskManager.Persistence.Repositories;

public class WorkflowRepository(IDbContext context) : GenericRepository<Workflow>(context)
    , IWorkflowRepository
{
    public async Task<WorkflowViewModel?> GetWorkflowViewModelByProjectIdAsync(Guid projectId)
    {
        var workflowViewModel = await Entity
            .AsNoTracking()
            .Where(w => w.ProjectId == projectId)
            .Select(w => new WorkflowViewModel
            {
                Id = w.Id,
                Name = w.Name,
            })
            .FirstOrDefaultAsync();

        return workflowViewModel;
    }
}
