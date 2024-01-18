namespace TaskManager.Core.Interfaces.Services;

public interface ITransitionService
{
    Task<IReadOnlyCollection<TransitionViewModel>> GetTransitionViewModelByProjectId(Guid projectId);
    Task<TransitionViewModel> CreateTransition(Guid projectId, CreateTransitionDto createTransitionDto);
    Task<TransitionViewModel> UpdateTransition(Guid id, UpdateTransitionDto updateWorkflowDto);
}
