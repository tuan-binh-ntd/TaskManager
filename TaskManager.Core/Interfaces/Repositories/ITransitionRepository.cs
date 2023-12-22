using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories;

public interface ITransitionRepository : IRepository<Transition>
{
    Transition Add(Transition transition);
    void Update(Transition transition);
    void Delete(Guid id);
    void AddRange(ICollection<Transition> transitions);
    Transition GetCreateTransitionByProjectId(Guid projectId);
    Task<Transition?> GetById(Guid id);
    Task<IReadOnlyCollection<TransitionViewModel>> GetByProjectId(Guid projectId);
}
