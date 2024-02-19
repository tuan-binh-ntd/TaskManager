namespace TaskManager.Persistence.Repositories;

public class WorkflowIssueTypeRepository(IDbContext context) : GenericRepository<WorkflowIssueType>(context)
    , IWorkflowIssueTypeRepository
{
    public async Task<IReadOnlyCollection<WorkflowIssueType>> GetWorkflowIssueTypesByWorkflowId(Guid workflowId)
    {
        var workflowIssueTypes = await Entity
            .Where(wit => wit.WorkflowId == workflowId)
            .ToListAsync();

        return workflowIssueTypes;
    }
}
