namespace TaskManager.Core.Interfaces.Repositories;

public interface IWorkflowIssueTypeRepository
{
    Task<IReadOnlyCollection<WorkflowIssueType>> GetWorkflowIssueTypesByWorkflowId(Guid workflowId);
}
