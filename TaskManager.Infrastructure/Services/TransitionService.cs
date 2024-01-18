namespace TaskManager.Infrastructure.Services;

public class TransitionService : ITransitionService
{
    private readonly ITransitionRepository _transitionRepository;

    public TransitionService(
        ITransitionRepository transitionRepository)
    {
        _transitionRepository = transitionRepository;
    }

    public async Task<TransitionViewModel> CreateTransition(Guid projectId, CreateTransitionDto createTransitionDto)
    {
        var transition = createTransitionDto.Adapt<Transition>();
        transition.ProjectId = projectId;
        _transitionRepository.Add(transition);
        await _transitionRepository.UnitOfWork.SaveChangesAsync();
        return transition.Adapt<TransitionViewModel>();
    }

    public async Task<IReadOnlyCollection<TransitionViewModel>> GetTransitionViewModelByProjectId(Guid projectId)
    {
        var transitionViewModels = await _transitionRepository.GetByProjectId(projectId);
        return transitionViewModels;
    }

    public async Task<TransitionViewModel> UpdateTransition(Guid id, UpdateTransitionDto updateWorkflowDto)
    {
        var transition = await _transitionRepository.GetById(id) ?? throw new TransitionNullException();
        transition.Name = updateWorkflowDto.Name;
        transition.FromStatusId = updateWorkflowDto.FromStatusId;
        transition.ToStatusId = updateWorkflowDto.ToStatusId;

        _transitionRepository.Update(transition);
        await _transitionRepository.UnitOfWork.SaveChangesAsync();
        return transition.Adapt<TransitionViewModel>();
    }
}
