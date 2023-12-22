using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Repositories;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Core.ViewModel;

namespace TaskManager.Infrastructure.Services;

public class WorkflowService : IWorkflowService
{
    private readonly ITransitionRepository _transitionRepository;

    private readonly IWorkflowRepository _workflowRepository;
    public WorkflowService(ITransitionRepository transitionRepository, IWorkflowRepository workflowRepository)
    {
        _transitionRepository = transitionRepository;
        _workflowRepository = workflowRepository;
    }

    public async Task<WorkflowViewModel> GetWorkflowViewModelByProjectId(Guid projectId)
    {
        var workflowViewModel = await _workflowRepository.GetByProjectId(projectId);
        workflowViewModel.Transitions = await _transitionRepository.GetByProjectId(projectId);
        return workflowViewModel;
    }

    public async Task UpdateWorkflow(Guid id, UpdateWorkflowDto updateWorkflowDto)
    {
        var workflowIssueTypes = await _workflowRepository.GetWorkflowIssueTypesByWorkflowId(id);
        _workflowRepository.RemoveWorkflowIssueTypeRange(workflowIssueTypes);

        var newWorkflowIssueTypes = new List<WorkflowIssueType>();
        if (updateWorkflowDto.IssueTypeIds.Count > 0)
        {
            foreach (var issueTypeId in updateWorkflowDto.IssueTypeIds)
            {
                var workflowIssueType = new WorkflowIssueType()
                {
                    WorkflowId = id,
                    IssueTypeId = issueTypeId,
                };
                newWorkflowIssueTypes.Add(workflowIssueType);
            }
        }

        _workflowRepository.AddWorkflowIssueTypeRange(newWorkflowIssueTypes);
        await _workflowRepository.UnitOfWork.SaveChangesAsync();
    }
}
