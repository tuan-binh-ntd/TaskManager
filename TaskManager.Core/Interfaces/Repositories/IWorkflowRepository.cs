using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories;

public interface IWorkflowRepository : IRepository<Workflow>
{
    Workflow Add(Workflow workflow);
    void Update(Workflow workflow);
    void Delete(Guid id);
    Task<WorkflowViewModel> GetByProjectId(Guid projectId);
    Task<IReadOnlyCollection<WorkflowIssueType>> GetWorkflowIssueTypesByWorkflowId(Guid workflowId);
    void AddWorkflowIssueTypeRange(IReadOnlyCollection<WorkflowIssueType> workflowIssueTypes);
    void RemoveWorkflowIssueTypeRange(IReadOnlyCollection<WorkflowIssueType> workflowIssueTypes);
}
