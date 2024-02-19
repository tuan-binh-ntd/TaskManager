namespace TaskManager.Core.Interfaces.Repositories;

public interface IWorkflowRepository
{
    Task<WorkflowViewModel?> GetWorkflowViewModelByProjectIdAsync(Guid projectId);
}
